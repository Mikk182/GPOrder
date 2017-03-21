function Group(params) {

    this.ReadyJS = function () {
        this.ApplyJQuery();
        this.SubscribeDeleteLinkClick();
    };

    this.SubscribeDeleteLinkClick = function () {
        $.each($(".deleteLink"), function (index, value) {
            $(this).click(function (event) {
                event.preventDefault();
                $(this).parent().remove();
            });
        });
    }

    this.ApplyJQuery = function () {

        //Lien 'Partager'
        $("#share").click(function () {
            $("#shareDialog").dialog();
        });
        $("#copyToClipboardButton").click(function () {
            $("#copyToClipboardText").select();
            document.execCommand("copy");
            //$("#shareDialog").dialog("close");
        });
        
        // Autocomplete
        $.each($("input[data-jqui-type='autocomplete']"),
            function (index, value) {
                $(value)
                    .autocomplete({
                        minLength: 0,
                        source: function (request, response) {
                            var url = value.dataset.jquiAcompSource;

                            $.getJSON(url,
                                { term: request.term },
                                function (data) {
                                    response(data);
                                });
                        },
                        select: function (event, ui) {
                            $(event.target).parent().children('input[type=hidden]').val(ui.item.id);
                        },
                        change: function (event, ui) {
                            if (!ui.item) {
                                $(event.target).val('').parent().children('input[type=hidden]').val('');
                            }
                        }
                    });
            });
    }
}