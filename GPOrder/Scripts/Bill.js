function Bill() {

    this.ReadyJS = function () {
        this.SubscribeAmountChangeEvent();
        this.SubscribeDeleteLinkClick();
    };

    this.SubscribeAmountChangeEvent = function () {
        $.each($("[id$='Amount']"), function () {
            $(this).on("change", function () {
                //event.preventDefault();
                var total = 0;
                $("[id$='Amount']").each(function () {
                    total = parseFloat($(this).val()) + total;
                });
                $("#total").text(total);
            });
        });
    }

    this.SubscribeDeleteLinkClick = function () {
        $.each($(".deleteLink"), function (index, value) {
            $(this).click(function (event) {
                event.preventDefault();
                $(this).parent().remove();
            });
        });
    }
}