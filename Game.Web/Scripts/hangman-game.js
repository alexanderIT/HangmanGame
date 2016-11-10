﻿
function clearTextBox() {
    var inputTextElement = document.getElementById("letter");
    inputTextElement.value = "";
}

var asyncSendKey = function () {
    function sendKey(e) {
        var form = $("#formLetters");

        var text = e.currentTarget.textContent;
        $("#hiddenInput").val(text);

        var options = {
            url: form.attr("action"),
            type: form.attr("method"),
            data: $("#hiddenInput").serialize(),
            contentType: "application/json"
        };

        $.ajax(options).done(function (data) {

            var target = $("#secretWord");
            $(target).html(data);
        })
        return false;
    }

    $(".container").on("click", ".key", sendKey);
}();
