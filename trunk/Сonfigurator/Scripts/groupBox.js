function dragmove() {
    d3.select(this)
        .style("top", ((d3.event.sourceEvent.pageY) - this.offsetHeight / 2) + "px")
        .style("left", ((d3.event.sourceEvent.pageX) - this.offsetWidth / 2) + "px");
}

function groupboxd3(groupId) {
    $("#btnSave").click(function () {
        var b = new Array();
        var collection = $("div.draggable");
        for (var i = 0; i < collection.length; i++) {
            var groupbox = collection[i];
            var groupBoxCoord = {

            };
            groupBoxCoord.groupboxId = groupbox.getAttribute("groupboxId");
            groupBoxCoord.w = Math.ceil($(groupbox).width());
            groupBoxCoord.h =Math.ceil($(groupbox).height());
            groupBoxCoord.t =Math.ceil($(groupbox).position().top);
            groupBoxCoord.l =Math.ceil($(groupbox).position().left);
            b[i] = groupBoxCoord;
        }
        //send server
        var data = JSON.stringify(b);
        $.ajax({
            method: "POST",
            url: "/GroupBox/SetGroupBoxCoord",
            dataType: "json",
            data: { groupBoxCoord: data },
            success: function (data, status, xhr) {
            }
        });
    });
    var drag = d3.behavior.drag()
        .on("drag", dragmove);

    $.ajax({
        method: "GET",
        url: "/GroupBox/GetGroupBoxs",
        dataType: "json",
        data: { groupId: groupId },
        success: function (data, status, xhr) {
            for (var i = 0; i < data.length; i++) {
                var x = Math.ceil(data[i].left);
                var y = Math.ceil(data[i].top);
                var w = Math.ceil(data[i].width);
                var h = Math.ceil(data[i].height);
                var text = data[i].name;
                var groupboxId = data[i].groupboxId;

                d3.select("body").append("div")
                    .attr("class", "draggable")
                    .attr("groupboxId", groupboxId)
                    .style('top', y).style('left', x).style("width", w).style("height", h)
                    .text(text)
                    .call(drag);
            }
            $(".draggable").resizable();
        }
    });
}