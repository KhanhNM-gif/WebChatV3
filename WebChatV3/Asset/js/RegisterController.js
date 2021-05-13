var Register = {

    init: function () {
        this.registerEvent();

    },
    registerEvent: function () {
        $(".login100-form-btn").off('click').on('click', async function (e) {
            e.preventDefault();
            var data = {
                "Email": $("input[name='email']").val(),
                "PhoneNumber": $("input[name='pass']").val(),
                "Pas": $("input[name='pass']").val(),
                "IsRememberPassword": false
            };
            console.log(data);

            const res = await fetch('/api/ApiUserRegister/Register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data),
            })
            const json = await res.json();

            console.log(json.Object.UserName);

        });
    }
}
Register.init();