function Bill() {

    this.ReadyJS = function () {
        this.SubscribeAmountChangeEvent();
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
}