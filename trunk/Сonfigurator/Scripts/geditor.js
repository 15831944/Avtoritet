function geditor(groupboxId) {
    var svgCanvas = document.querySelector('svg'),
        svgNS = 'http://www.w3.org/2000/svg',
        rectangles = [];

    function Rectangle(x, y, w, h,text,svgCanvas) {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
        this.stroke = 1;
        this.el = document.createElementNS(svgNS, 'rect');
        this.txt = document.createElementNS(svgNS, 'text');
        this.text = text;

        this.el.setAttribute('data-index', rectangles.length);
        this.el.setAttribute('class', 'edit-rectangle');

        rectangles.push(this);

        this.draw();

        debugger;
        svgCanvas.appendChild(this.el);
        svgCanvas.appendChild(this.txt);
    }

    Rectangle.prototype.draw = function() {
        this.el.setAttribute('x', this.x + this.stroke / 2);
        this.el.setAttribute('y', this.y + this.stroke / 2);
        this.el.setAttribute('width', this.w - this.stroke);
        this.el.setAttribute('height', this.h - this.stroke);
        this.el.setAttribute('stroke-width', this.stroke);

        this.txt.setAttribute('text','!!!!!!!!!!!!');
        this.txt.setAttribute('x', this.x + 10);
        this.txt.setAttribute('y', this.y + 10);
        this.txt.setAttribute('fill', 'white');
        this.txt.setAttribute('stroke-width', this.stroke);



    }

    interact('.edit-rectangle')
        // change how interact gets the
        // dimensions of '.edit-rectangle' elements
        .rectChecker(function(element) {
            // find the Rectangle object that the element belongs to
            var rectangle = rectangles[element.getAttribute('data-index')];

            // return a suitable object for interact.js
            return {
                left: rectangle.x,
                top: rectangle.y,
                right: rectangle.x + rectangle.w,
                bottom: rectangle.y + rectangle.h
            };
        })
        .inertia({
            // don't jump to the resume location
            // https://github.com/taye/interact.js/issues/13
            zeroResumeDelta: true
        })
        .restrict({
            // restrict to a parent element that matches this CSS selector
            drag: 'svg',
            // only restrict before ending the drag
            endOnly: true,
            // consider the element's dimensions when restricting
            elementRect: { top: 0, left: 0, bottom: 1, right: 1 }
        })
        .draggable({
            max: Infinity,
            onmove: function(event) {
                var rectangle = rectangles[event.target.getAttribute('data-index')];

                rectangle.x += event.dx;
                rectangle.y += event.dy;
                rectangle.draw();
            }
        })
        .resizable({
            max: Infinity,
            onmove: function(event) {
                var rectangle = rectangles[event.target.getAttribute('data-index')];

                rectangle.w = Math.max(rectangle.w + event.dx, 10);
                rectangle.h = Math.max(rectangle.h + event.dy, 10);
                rectangle.draw();
            }
        });

    interact.maxInteractions(Infinity);
    $.ajax({
        method: "GET",
        url: "/Brand/GetBrands",
        dataType:"json",
        data: { groupboxId: groupboxId },
        success: function (data, status, xhr) {
            debugger;
            for (var i = 0; i < data.length; i++) {
                var x = data[i].left;
                var y = data[i].top;
                var text = data[i].name;

                new Rectangle(x,y,30,20,name,svgCanvas);
            }
        }
    });
}