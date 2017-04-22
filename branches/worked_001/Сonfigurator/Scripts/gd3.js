function dragmove() {
    d3.select(this)
        .style("top", ((d3.event.sourceEvent.pageY) - this.offsetHeight / 2) + "px")
        .style("left", ((d3.event.sourceEvent.pageX) - this.offsetWidth / 2) + "px");
}

function gd3(groupboxId) {

    $("#btnSave").click(function () {
        var b = new Array();
        var collection = $("div.draggable");
        for (var i = 0; i < collection.length; i++) {
            var brand = collection[i];
            var brandCoord = {

            };
            brandCoord.brandId = brand.getAttribute("brandId");
            brandCoord.w = Math.ceil($(brand).width());
            brandCoord.h = Math.ceil($(brand).height());
            brandCoord.t = Math.ceil($(brand).position().top);
            brandCoord.l = Math.ceil($(brand).position().left);
            b[i] = brandCoord;
        }
        //send server
        var data = JSON.stringify(b);
        $.ajax({
            method: "POST",
            url: "/Brand/SetBrandCoord",
            dataType: "json",
            data: {brandcoord:data },
            success: function (data, status, xhr) {
            }
        });
    });
    var drag = d3.behavior.drag()
        .on("drag", dragmove);

    $.ajax({
        method: "GET",
        url: "/Brand/GetBrands",
        dataType: "json",
        data: { groupboxId: groupboxId },
        success: function(data, status, xhr) {
            for (var i = 0; i < data.length; i++) {
                var x = Math.ceil(data[i].left);
                var y = Math.ceil(data[i].top);
                var w = Math.ceil(data[i].width);
                var h = Math.ceil(data[i].height);
                var text = data[i].name;
                var brandId = data[i].brandId;

                d3.select("body").append("div")
                    .attr("class", "draggable")
                    .attr("brandId", brandId)
                    .style('top', y).style('left', x).style("width", w).style("height", h)
                    .text(text)
                    .call(drag);
            }
            $(".draggable").resizable();
        }
    });
}