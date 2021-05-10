var Modal = {
    init: async function () {
        this.registerEvent();
    },
    registerEvent: function () {
        $("#btn-create-server").off('click').on('click', async function (e) {
            e.preventDefault();
            var Authorization = localStorage.getItem("Authorization");
            if (Authorization != null) {
                const input = document.getElementById('upload-photo');
                const json = await FileUpload.upload(input.files[0]);

                var data = {
                    "Name": $("input[name='name-new-server']").val(),
                    "Image": json.Object[0].FileUrl
                };

                const res = await fetch('/api/ApiServer/InsertOrUpdate', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': Authorization
                    },
                    body: JSON.stringify(data),
                })
                const json2 = await res.json();
                var list = [json2.Object];
                ChatingIndex.GenerateAllServerHTML(list, false);
                $('#modal-create-server').modal('hide');
            }
            else {
                window.location.replace("/UserRegister/Login");

            }
        });
        $(".close-modal").off('click').on('click', async function (e) {
            e.preventDefault();
            $('#modal-create-server').modal('hide');
        })
    }
}
Modal.init();