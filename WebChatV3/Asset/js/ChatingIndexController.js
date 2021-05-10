var ChatingIndex = {
    LoadWeb: async function () {
        var splitURL = window.location.href.split("/");
        var room = splitURL.pop();
        var server = splitURL.pop();
        if (server == 'channels') {
            if (room == '@me') {
                ChatingIndex.init();
            }
            else {
                ChatingIndex.init();
                ChatingIndex.ClickServer(room, '')
            }
        }
        else {
            if (server == '@me') {
                ChatingIndex.init();
                var json;
                if (Authorization != null) {
                    const res = await fetch('/api/ApiUserRegister/GetOneByObjectFriendShip?GuidFriend=' + room, {
                        method: 'GET',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': Authorization
                        }
                    });
                    json = await res.json();
                    Login.Authentication(json);
                    ChatingIndex.ClickFriend(json.Object.ObjectGuid);
                }
                else window.location.replace("/UserRegister/Login");

            }
            else {
                ChatingIndex.init();
                ChatingIndex.ClickServer(server, room);
            }
        }
    },
    init: async function () {
        await this.LoadPage();
        this.registerEvent();
    },
    registerEvent: function () {
        $(".clickFriend").off('click').on('click', async function (e) {
            e.preventDefault();
            var href = $(this).attr('href');
            GuidUser = $(this).data("id");
            ChatingIndex.ClickFriend(href, GuidUser);
        });
        $.contextMenu({
            selector: '.clickFriend',
            callback: function (key, options) {
                var m = "clicked: " + $(this).html();
                window.console && console.log(m) || alert(m);
            },
            items: {
                "Xem hồ sơ": { name: "view" },
                "Xóa bạn": { name: "remove" },
                "Chặn": { name: "ban" },
            }
        });

        $(".brand-link").off('click').on('click', async function (e) {
            e.preventDefault();
            history.pushState(null, "Trang Chủ", '/channel/@me');
            await ChatingIndex.GenerateSlideBarMe();
            await ChatingIndex.GetListFriends();
            ChatingIndex.registerEvent();
        });
        $(".dropdown-toggle").off('click').on('click', async function (e) {
            e.preventDefault();
            if ($("#dropdownMenuEditServer").hasClass("show"))$("#dropdownMenuEditServer").removeClass("show");
            else $("#dropdownMenuEditServer").addClass("show");
        });
        $("#nof-message").off('click').on('click', async function (e) {
            e.preventDefault();
            if ($("#dropdown-nof-message").hasClass("show")) $("#dropdown-nof-message").removeClass("show");
            else $("#dropdown-nof-message").addClass("show");
        });
        $("#nof-notifications").off('click').on('click', async function (e) {
            e.preventDefault();
            if ($("#dropdown-nof-notifications").hasClass("show")) $("#dropdown-nof-notifications").removeClass("show");
            else $("#dropdown-nof-notifications").addClass("show");
        });
        $(".click-channel").off('click').on('click', async function (e) {
            e.preventDefault();
            var href = $(this).attr('href');
            var hrefSplit = href.split("/");
            ChatingIndex.ClickChannel(hrefSplit.pop(), hrefSplit.pop())
        });

        $(".click-server").off('click').on('click', async function (e) {
            e.preventDefault();
            var href = $(this).attr('href');
            var title = $(this).attr('title');
            var hrefSplit = href.split("/");
            history.pushState(null, "kenh", '/channels/' + href);
            if (hrefSplit.length == 2) ChatingIndex.ClickServer(hrefSplit[0], hrefSplit[1]);
            else hatingIndex.ClickServer(hrefSplit[0],'');
            
        });
        $('#logout').off('click').on('click', async function (e) {
            e.preventDefault();
            var Authorization = localStorage.getItem("Authorization");
            const res = await fetch('/api/ApiUserRegister/Logout', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': Authorization
                }
            })
            const json = await res.json();

            console.log(json);

            if (json.Status == 0) {
                localStorage.removeItem("Authorization");
                window.location.replace("/UserRegister/Login");
            }
            else {
                alert(json.Object);
            }

        });
    },

    LoadPage: async function () {
        await this.GetListFriends();
        await this.GetListServer();
        this.ConnectHub();
    },
    ConnectHub: async function () {
        // Reference the auto-generated proxy for the hub.
        var chat = $.connection.chatHub;
        // Create a function that the hub can call back to display messages.
        chat.client.addNewMessageToPage = function (json) {
            var splitURL = window.location.href.split("/");
            var room = splitURL.pop();

            if (room == json[0].RoomGuid) ChatingIndex.GenerateMessage(json, false);
            else ChatingIndex.GenerateNofMessage(json, false)
        };
        // Set initial focus to message input box.
        $('#div_sendMsg').focus();
        // Start the connection.
        $.connection.hub.start().done(async function () {
            await chat.server.connect(localStorage.getItem("Authorization"));

            $('#i_sendMessage').click(async function () {
                ChatingIndex.SendMessage();
            });

            $('#div_sendMsg').keypress(async function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault();
                    ChatingIndex.SendMessage();
                }
            });
        });
    },
    SendMessage: async function () {
        var splitURL = window.location.href.split("/");

        var data = {
            "ConnectionID": $.connection.hub.id,
            "RoomGuid": splitURL.pop(),
            "MessageContent": $('#div_sendMsg').html()
        };

        if (splitURL[splitURL.length - 1] != "@me") {
            data["IsMessagePrivate"] = false;
            data["GuidServer"] = splitURL[splitURL.length - 1];
        }
        else {
            data["IsMessagePrivate"] = true;
        }

        const res = await fetch('/api/ApiChating/SendMessageToServer', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': localStorage.getItem("Authorization"),
                body: data
            },
            body: JSON.stringify(data)
        });

        // Clear text box and reset focus for next comment.
        $('#div_sendMsg').html('').focus();
    },
    ClickFriend: async function (href, GuidUser) {
        $(".tabPeople").show();
        $(".member-comlumn").hide();

        var Authorization = localStorage.getItem("Authorization");
        
        history.pushState(null, "chat riêng", '/channels/@me/' + href);

        

        if (Authorization != null) {
            const res1 = await fetch('/api/ApiUserRegister/GetOne?GuidUser=' + GuidUser, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': Authorization
                }
            });
            const json1 = await res1.json();
            $('#title-chatbox').html('<i class="fas fa-at px-1"></i> ' + json1.Object.Name);

            const res = await fetch('/api/ApiMessage/LoadPrivateMessage?GuidUser=' + GuidUser, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': Authorization
                }
            });
            const json = await res.json();

            ChatingIndex.GenerateMessage(json.Object, true);
        }
        else {
            window.location.replace("/UserRegister/Login");
        }
    },
    ClickServer: async function (GuidServer,GuidChannel) {

        $(".tabPeople").show();
        $(".member-comlumn").show();

        var Authorization = localStorage.getItem("Authorization");
        if (Authorization != null) {
            
            const res = await fetch('/api/ApiServer/GetOne?GuidServer=' + GuidServer, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': Authorization
                }
            })
            const json = await res.json();

            const res2 = await fetch('/api/ApiChannel/GetList?GuidServer=' + GuidServer, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': Authorization
                }
            });
            const json2 = await res2.json();
            json2.Object.forEach(function (element) {
                element["ObjectGuidServer"] = json.Object.ObjectGuid;
            });
            await ChatingIndex.GenerateSlideBarServer(json2.Object,true);

            const res3 = await fetch('/api/ApiMember/GetList?GuidServer=' + GuidServer, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': Authorization
                }
            });
            const json3 = await res3.json();
            await ChatingIndex.GenerateMemberOffline(json3.Object.outLtOfflineMember, true);
            await ChatingIndex.GenerateMemberOnline(json3.Object.outLtOnlineMember, true);

            if (GuidChannel!='')ChatingIndex.ClickChannel(GuidChannel, GuidServer);
            ChatingIndex.registerEvent();
        }
        else {
            window.location.replace("/UserRegister/Login");
        }
    },
    ClickChannel: async function (GuidChannel, GuidServer) {
        var Authorization = localStorage.getItem("Authorization");
        if (Authorization != null) {
            $(".tabPeople").show();
            const res1 = await fetch('/api/ApiChannel/GetOne?GuidServer=' + GuidServer + '&GuidChannel=' + GuidChannel, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': Authorization
                }
            });
            const json1 = await res1.json();
            $('#title-chatbox').html('<i class="fas fa-hashtag px-1"></i> ' + json1.Object.Name);

            const res = await fetch('/api/ApiMessage/LoadPublicMessage?GuidServer=' + GuidServer + '&GuidChannel=' + GuidChannel, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': Authorization
                }
            });
            const json = await res.json();

            ChatingIndex.GenerateMessage(json.Object, true);
        }
        else {
            window.location.replace("/UserRegister/Login");
        }
    },
    GetListFriends: async function () {
        var Authorization = localStorage.getItem("Authorization");
        if (Authorization != null) {
            const res = await fetch('/api/ApiFriendship/GetListFriends', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': Authorization
                },
            })
            const json = await res.json();
            this.GenerateAllFriendsHTML(json.Object);
        }
        else {
            window.location.replace("/UserRegister/Login");
        }
    },
    GenerateAllFriendsHTML: function (data) {
        var list = $('.listfen .list-friend');
        var theTemplateScript = $("#friend-template").html();
        var theTemplate = Handlebars.compile(theTemplateScript);
        list.append(theTemplate(data));
    },
    GetListServer: async function () {
        var Authorization = localStorage.getItem("Authorization");
        if (Authorization != null) {
            const res = await fetch('/api/ApiServer/GetListServer', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': Authorization
                }
            })
            const json = await res.json();
            console.log(json.Object);
            this.GenerateAllServerHTML(json.Object,true)
        }
        else {
            window.location.hash = "/UserRegister/Login";
        }
    },
    GenerateAllServerHTML: function (data, isReplace) {
        var list = $('.list-server .slide-bar-server');
        var theTemplateScript = $("#server-template").html();
        var theTemplate = Handlebars.compile(theTemplateScript);
        if (isReplace) {
            list.html(theTemplate(data));
        }
        else {
            list.append(theTemplate(data))
        };
    },
    GenerateMessage: function (data, isReplace) {
        var list = $('.card .message');
        var theTemplateScript = $("#message-template").html();
        var theTemplate = Handlebars.compile(theTemplateScript);
        if (isReplace) list.html(theTemplate(data));
        else list.append(theTemplate(data));
    },
    GenerateSlideBarMe: function () {
        var list = $('.main-sidebar .privateChannels');
        var theTemplateScript = $("#me-slide-bar-template").html();
        var theTemplate = Handlebars.compile(theTemplateScript);
        list.html(theTemplate());
    },
    GenerateSlideBarServer: function (data, isReplace) {
        var list = $('.main-sidebar .privateChannels');
        var theTemplateScript = $("#server-slide-bar-template").html();
        var theTemplate = Handlebars.compile(theTemplateScript);
        if (isReplace) {
            list.html(theTemplate(data));
        }
        else {
            list.append(theTemplate(data))
        };
    },
    GenerateMemberOnline: function (data, isReplace) {
        var number = $('#number-member-online');

        var list = $('.member-comlumn .list-online');
        var theTemplateScript = $("#member-online-template").html();
        var theTemplate = Handlebars.compile(theTemplateScript);

        if (number.html() == '') number.html('0');
        if (isReplace) {
            list.html(theTemplate(data));
            number.html(parseInt(data.length));
        }
        else {
            list.append(theTemplate(data))
            number.html(parseInt(number.html()) + data.length);
        };
    },
    GenerateMemberOffline: function (data, isReplace) {
        var number = $('#number-member-offline');

        var list = $('.member-comlumn .list-offline');
        var theTemplateScript = $("#member-offline-template").html();
        var theTemplate = Handlebars.compile(theTemplateScript);

        if (number.html() == '') number.html('0');
        if (isReplace) {
            list.html(theTemplate(data));
            number.html(parseInt(data.length));
        }
        else {
            list.append(theTemplate(data))
            number.html(parseInt(number.html()) + data.length);
        };
    },
    GenerateNofMessage: function (data, isReplace) {
        var number = $('#number-nof-mes');

        var list = $('#dropdown-nof-message');
        var theTemplateScript = $("#nof-message-template").html();
        var theTemplate = Handlebars.compile(theTemplateScript);

        if (number.html() == '') number.html('0');
        if (isReplace) {
            list.html(theTemplate(data));
            number.html(parseInt(data.length));
        }
        else {
            list.append(theTemplate(data))
            number.html(parseInt(number.html()) + data.length);
        };
    },
    GenerateRoleServer: function (data) {
        var list = $('#dropdownMenuEditServer');
        var theTemplateScript = $("#role-server-template").html();
        var theTemplate = Handlebars.compile(theTemplateScript);
        list.html(theTemplate(data));
    }
};
ChatingIndex.LoadWeb();

Handlebars.registerHelper('if_eq', function (a, b, opts) {
    if (a == b) {
        return opts.fn(this);
    } else {
        return opts.inverse(this);
    }
});
Handlebars.registerHelper('if_user', function (a, opts) {
    var AccountUser = parseInt(localStorage.getItem("AccountUser"));
    if (a == AccountUser) {
        return opts.fn(this);
    } else {
        return opts.inverse(this);
    }
});

Handlebars.registerHelper("formatDate", function (datetime, format) {
    return moment(new Date(datetime)).fromNow();
});

window.addEventListener('popstate', (event) => {
    console.log("location: " + document.location + ", state: " + JSON.stringify(event.state))
});
$(".tabPeople").hide();

window.addEventListener('popstate', (e) => ChatingIndex.LoadWeb());