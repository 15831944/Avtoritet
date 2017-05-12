function brandeditor(groupboxId) {

    $("#btnSave").click(function () {
        var b = new Array();
        var collection = $("div.draggable");
        for (var i = 0; i < collection.length; i++) {
            var brand = collection[i];
            var brandCoord = {

            };
            brandCoord.brandId = brand.getAttribute("brandId");
            brandCoord.w = $(brand).width();
            brandCoord.h = $(brand).height();
            brandCoord.t = $(brand).position().top;
            brandCoord.l = $(brand).position().left;
            b[i] = brandCoord;

        }
        //send server
        var data = JSON.stringify(b);
        $.ajax({
            method: "POST",
            url: "/Brand/SetBrandCoord",
            dataType: "json",
            data: { brandcoord: data },
            success: function (data, status, xhr) {
            }
        });
    });

    $.ajax({
        method: "GET",
        url: "/Brand/GetBrands",
        dataType: "json",
        data: { groupboxId: groupboxId },
        success: function (data, status, xhr) {
            for (var i = 0; i < data.length; i++) {
                var x = data[i].left;
                var y = data[i].top;
                var w = data[i].width;
                var h = data[i].height;
                var text = data[i].name;
                var brandId = data[i].brandId;
                var newdiv = document.createElement("div");
                $(newdiv).attr("class", "draggable,content-widget")
                    .attr("brandId", brandId)
                    .css("top", y)
                    .css("left", x)
                    .css("width", w)
                    .css("height", h).text(text);

                $(newdiv).draggable();
                $(newdiv).resizable();
                $("body").append(newdiv);
            }
        }
    });
}