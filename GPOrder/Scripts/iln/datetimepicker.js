var DatetimePicker = DatetimePicker || {};

DatetimePicker = function () {
}

DatetimePicker.loadDatetimePickers = function () {
    $('.iln-datetimepicker').each(function () {
        DatetimePicker.loadDatetimePicker($(this));
    });
}
DatetimePicker.loadDatetimePicker = function (element) {
    if ($(element).data('load'))
        return;
    $(element).data('load', true);
    
    var id = $(element).data('iln-wrapperid');
    $('#' + id).datetimepicker({
        allowInputToggle: $(element).data('allowinputtoggle'),
        calendarWeeks: $(element).data('calendarweeks'),
        collapse: $(element).data('collapse'),
        daysOfWeekDisabled: eval($(element).data('daysofweekdisabled') + ''),
        dayViewHeaderFormat: $(element).data('dayviewheaderformat'),
        debug: $(element).data('debug'),
        defaultDate: $(element).data('value'),
        disabledTimeIntervals: $(element).data('disabledtimeintervals'),
        enabledDates: $(element).data('enableddates'),
        enabledHours: $(element).data('enabledhours'),
        extraFormats: $(element).data('extraformats'),
        focusOnShow: $(element).data('focusonshow'),
        format: $(element).data('format'),
        inline: $(element).data('inline'),
        keepInvalid: $(element).data('keepinvalid'),
        keepOpen: $(element).data('keepopen'),
        locale: $(element).data('locale'),
        maxDate: $(element).data('maxdate'),
        minDate: $(element).data('mindate'),
        showClear: $(element).data('showclear'),
        showClose: $(element).data('showclose'),
        showTodayButton: $(element).data('showtodaybutton'),
        sideBySide: $(element).data('sidebyside'),
        stepping: $(element).data('stepping') * 1,
        toolbarPlacement: $(element).data('toolbarplacement'),
        tooltips: $(element).data('tooltips'),
        useCurrent: $(element).data('usecurrent'),
        useStrict: $(element).data('usestrict'),
        viewMode: $(element).data('viewmode'),
        viewDate: $(element).data('viewdate'),
        widgetPositioning: $(element).data('widgetpositioning'),
        icons: $(element).data('icons')
    });
}

DatetimePicker.verifyNumber = function (e) {
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190])
               !== -1 || e.keyCode == 65 && e.ctrlKey === true
        || e.keyCode >= 35 && e.keyCode <= 39) {
        return
    } if ((e.shiftKey || e.keyCode < 48 || e.keyCode > 57) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault()
    }
}

DatetimePicker.format = function (mascara, documento) {
    var i = documento.value.length;
    var saida = mascara.substring(0, 1);
    var texto = mascara.substring(i);
    if (texto.substring(0, 1) != saida) {
        documento.value += texto.substring(0, 1);
    }
}

$(document).ready(function () {
    DatetimePicker.loadDatetimePickers();
});