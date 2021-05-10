var FileUpload = {
    registerEvent: function () {
        $("#upload-photo").change(function () {
            FileUpload.readURL(this);
        })
    },
    readURL: function(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.readAsDataURL(input.files[0]); // convert to base64 string
            reader.onload = function (e) {
                    $('#img-choose').attr('src', e.target.result);
            }
        }
    },
    upload: async function (file) {
        var formData = new FormData();
        formData.append("file", file);

        var Authorization = localStorage.getItem("Authorization");
        var rep;
        await $.ajax({
            url: '/api/ApiUploadFile/UploadFile',
            type: 'POST',
            headers: {
                'Authorization': Authorization
            },
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.Status == 0) {
                    rep= response;
                }
                else {
                    alert(response.Object);
                }
            },
        });
        return rep;
    }
}
FileUpload.registerEvent();