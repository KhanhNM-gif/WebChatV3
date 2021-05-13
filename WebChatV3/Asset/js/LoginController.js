var Login = {

    init: function () {
        this.registerEvent();

    },
    registerEvent: function () {
        $(".login100-form-btn").off('click').on('click', async function (e) {
            e.preventDefault();
            var data = {
                "UserName": $("input[name='email']").val(),
                "Password": $("input[name='pass']").val(),
                "IsRememberPassword": false
            };
            console.log(data);

            const res = await fetch('/api/ApiUserRegister/Login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data),
            })
            const json = await res.json();

        });
    }
}
Login.init();