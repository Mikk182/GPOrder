function Order(params) {

    this.ReadyJS = function () {
        this.ApplyJQuery();
        this.SubscribeDeleteLinkClick();
        this.InitWatermark();
    };

    this.InitWatermark = function () {
        if (params.IsOnCreation === true) {
            var firstInput = $("[id ^='OrderLines'][id $='Description']").first();
            firstInput.addClass("watermark");
            var firstInputValue = null;
            firstInput.focus(function () {
                if ($(this).hasClass("watermark")) {
                    firstInputValue = $(this).val();
                    $(this).val("").removeClass("watermark");
                }
            });
            firstInput.blur(function(){
                if ($(this).val().length === 0 && firstInputValue) {
                    $(this).val(firstInputValue).addClass('watermark');
                }
            });
        }
    }

    this.SubscribeDeleteLinkClick = function () {
        $.each($(".deleteLink"), function (index, value) {
            $(this).click(function (event) {
                event.preventDefault();
                $(this).parent().remove();
            });
        });
    }

    this.ApplyJQuery = function () {
        $.each($("#OrderLinesDiv input[data-jqui-type='autocomplete']"),
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