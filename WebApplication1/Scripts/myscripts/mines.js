$(document).ready(function () {

    var updateResources = function () {
        updateResource("Clay");
        updateResource("Wheat");
        updateResource("Iron");
        updateResource("Wood");
    };
    var updateResource = function (resorceName) {
        var start = new Date();
        var currentProduction = 0;
        var currentValue = parseFloat($(".res-value." + resorceName).text());
        console.log(currentValue);
        var lastUpdate = Date.parse($(".res-update." + resorceName).text());
        //console.log(lastUpdate );
        var mines = $(".mines").find("." + resorceName);
        $.each(mines, function (index, value)
        {
            currentProduction += parseInt($(value).find(".hourProduction").text());
        }
        );
       // console.log(currentProduction);
        var nextValue = (currentValue + ((start.getTime() - lastUpdate) / 1000 / 60 / 60) * currentProduction).toFixed(4);
        $(".res-value." + resorceName).text(nextValue);
        $(".res-update." + resorceName).text(start.strftime("%m/%d/%Y %H:%M:%S"));
    };
    setInterval(updateResources, 1000);
    //afis
    var getMineDetailsHTML = function (mineId) {
        $('#mine-details-container > .content').empty();
        $('#mine-details-container > .content').load("/Mines/Details?mineId=" + mineId);
        $('#mine-details-container').addClass('show');
    };
    $('.mine-details-btn').click(function (e)
    {
        var mineId = $(this).data('mine-id');
        getMineDetailsHTML(mineId);
    })
    //inchide
    $('#mine-details-container > .close-btn').click(function () {
        $('#mine-details-container').removeClass('show');
    })
});