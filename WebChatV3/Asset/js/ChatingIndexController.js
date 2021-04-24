var ChatingIndex = {
    init: async function () {
        await this.getListFriends();
        this.registerEvent();

    },
    registerEvent: function () {
            $(".clickFriend").off('click').on('click', async function (e) {
                e.preventDefault();
                var Authorization = localStorage.getItem("Authorization");
                var href = $(this).attr('href');
                
                    history.pushState(null, "chat riêng", 'https://localhost:44387/chating/' + href);
                
                var test=window.location;
                var res1 = href.split("/");
                if (Authorization != null) {
                    const res = await fetch('/api/ApiMessage/LoadMessage?GuidUser=' + href, {
                        method: 'GET',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': Authorization
                        }
                    });
                    const json = await res.json();
                    ChatingIndex.generateMessage(json.Object);
                }
                else {
                    window.location.replace("/UserRegister/Login");
                }

            })
    },
    getListFriends: async function () {
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
            this.generateAllFriendsHTML(json.Object);
        }
        else {
            window.location.replace("/UserRegister/Login");
        }
    },
    generateAllFriendsHTML: function (data) {
        var list = $('.listfen .list-friend');
        var theTemplateScript = $("#friend-template").html();
        var theTemplate = Handlebars.compile(theTemplateScript);
        list.append(theTemplate(data));
    },
    getListServer: async function () {
        var Authorization = localStorage.getItem("Authorization");
        if (Authorization != null) {
            const res = await fetch('/api/ApiChating/getListServer', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': Authorization
                }
            })
            const json = await res.json();
            console.log(json.Object);
            this.generateAllServerHTML(json.Object)
        }
        else {
            window.location.hash = "/UserRegister/Login";
        }
    },
    generateAllServerHTML: function (data) {
        var list = $('.fisrtbar .list-server');
        var theTemplateScript = $("#server-template").html();
        var theTemplate = Handlebars.compile(theTemplateScript);
        list.append(theTemplate(data));
    },
    generateMessage: function (data) {
        var list = $('.card .message');
        var theTemplateScript = $("#message-template").html();
        var theTemplate = Handlebars.compile(theTemplateScript);
        list.append(theTemplate(data));
    },


};
Handlebars.registerHelper('if_eq', function (a, b, opts) {
    if (a == b) {
        return opts.fn(this);
    } else {
        return opts.inverse(this);
    }
});
window.addEventListener('popstate', (event) => {
    console.log("location: " + document.location + ", state: " + JSON.stringify(event.state))
});
window.onpopstate = function (event) {
    console.log("location: " + document.location + ", state: " + JSON.stringify(event.state));
};
ChatingIndex.init();