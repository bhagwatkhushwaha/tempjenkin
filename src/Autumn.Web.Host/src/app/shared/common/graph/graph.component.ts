import { Component, OnInit, Input } from '@angular/core';
import { accountModuleAnimation } from '@shared/animations/routerTransition';

declare var $: any;

@Component({
    selector: 'canvas-graph',
    templateUrl: './graph.component.html',
    styleUrls: ['./graph.component.scss'],
    animations: [accountModuleAnimation()]
})

export class GraphComponent implements OnInit {

    @Input('data') data: any;
    @Input('user') user: any;

    _lineType = { solid: 'solid', dashed: 'dashed' };
    _context;
    _data;
    _numXPoints = 0;
    _numYPoints = 0;
    _font = "12pt Calibri";
    _padding = 25;
    _axisColor = "#555";
    _fontHeight = 12;
    _xStart = 0;
    _yStart = 0;
    _width = 0;
    _height = 0;
    _prevX = 0;
    _prevY = 0;

    minSLiderVal: number;
    maxSLiderVal: number;
    desiredRetirementAge: number;

    constructor() {

    }

    ngOnInit() {
        this.renderCanvasChart();
        this.generateSlider();
    }

    render(canvasId, dataObj) {
        //Assign properties
        this._data = dataObj;

        var canvas: any = document.getElementById(canvasId);
        this._context = canvas.getContext("2d");
        this._context.clearRect(0, 0, canvas.width, canvas.height);

        this._numXPoints = Math.round((this._data.maxX - this._data.minX) / this._data.diffPerPointX);
        this._numYPoints = Math.round((this._data.maxY - this._data.minY) / this._data.diffPerPointY);

        this._xStart = this.getLongestValueWidth() + this._padding * 2;
        this._yStart = this._padding * 2;

        this._width = canvas.width - this._xStart - this._padding * 2;
        this._height = canvas.height - this._yStart - this._padding - this._fontHeight;

        this.renderChart();
    }

    renderChart() {
        //Render X axis
        this.draw_X_Axis();

        //Render Y axis
        this.draw_Y_Axis();

        //Render lines
        for (var line = 0; line < this._data.dataPoints.length; line++) {
            var linePoints = this._data.dataPoints[line];
            this.renderLines(linePoints);
            this.renderIntersectionPoints(linePoints);
        }

        //Render connecting points
        //renderConnectingPoints(linePoints);
    }

    draw_X_Axis() {
        this._context.save();
        this._context.beginPath();

        // Draw x axis line
        this._context.moveTo(this._xStart, this._yStart + this._height);
        this._context.lineTo(this._xStart + this._width, this._yStart + this._height);
        this._context.strokeStyle = this._axisColor;
        this._context.lineWidth = 2;
        this._context.stroke();

        // Draw tick marks
        // for (var n = 0; n < _numXPoints; n++) {
        //     _canvas.beginPath();
        //     _canvas.moveTo((n + 1) * _width / _numXPoints + _xStart, _yStart + _height);
        //     _canvas.lineTo((n + 1) * _width / _numXPoints + _xStart, _yStart + _height - _tickSize);
        //     _canvas.stroke();
        // }

        // Draw labels
        this._context.font = this._font;
        this._context.fillStyle = "black";
        this._context.textAlign = "center";
        this._context.textBaseline = "middle";

        for (var n = 0; n < this._numXPoints + 1; n++) {

            var label = this._data.minX + (this._data.diffPerPointX * (n * 1));
            this._context.save();

            var diffTwoXPoints = this._width / this._numXPoints;
            this._context.translate((n * diffTwoXPoints) + this._xStart, this._yStart + this._height + this._padding);
            this._context.fillText(label, 0, 0);
            this._context.restore();
        }

        this._context.restore();
    }

    draw_Y_Axis() {
        this._context.save();
        this._context.beginPath();

        this._context.moveTo(this._xStart, this._yStart);
        this._context.lineTo(this._xStart, this._yStart + this._height);
        this._context.strokeStyle = this._axisColor;
        this._context.lineWidth = 2;

        this._context.stroke();
        this._context.restore();

        //draw tick marks
        // for (var n = 0; n < _numYPoints; n++) {
        //     _context.beginPath();
        //     _context.moveTo(_xStart, n * _height / _numYPoints + _yStart);
        //     _context.lineTo(_xStart + _width, n * _height / _numYPoints + _yStart);
        //     _context.stroke();
        // }

        // draw lable
        this._context.font = this._font;
        this._context.fillStyle = "black";
        this._context.textAlign = "right";
        this._context.textBaseline = "middle";

        for (var n = 0; n < this._numYPoints + 1; n++) {
            var label = this._data.minY + (this._data.diffPerPointY * n);
            label = "$" + label;
            this._context.save();
            var disp = (this._height / this._numYPoints) * n;
            this._context.translate(this._xStart - this._padding, this._yStart + this._height - disp);
            this._context.fillText(label, 0, 0);
            this._context.restore();
        }

        this._context.restore();
    }

    renderLines(linePoints) {
        this._prevX = 0;
        this._prevY = 0;

        for (var i = 0; i < linePoints.length; i++) {
            var pt = linePoints[i];
            //Calculate x position
            var diffTwoXPoints = this._width / this._numXPoints;
            var ptX = (pt.x - this._data.minX) * (diffTwoXPoints / this._data.diffPerPointX) + this._xStart;
            //Calculate y position
            var diffTwoYPoints = this._height / this._numYPoints;
            var ptY = (this._yStart + this._height) - (pt.y - this._data.minY) * (diffTwoYPoints / this._data.diffPerPointY);

            if (i > 0) {
                //Draw connecting lines
                var lineDash = (pt.lineType == this._lineType.dashed) ? [15, 4] : [];
                this.drawLine(ptX, ptY, this._prevX, this._prevY, pt.color, 4, lineDash);
            }

            this._prevX = ptX;
            this._prevY = ptY;
        }
    }

    renderIntersectionPoints(linePoints) {
        for (var i = 0; i < linePoints.length; i++) {
            var pt = linePoints[i];
            //Calculate x position
            var diffTwoXPoints = this._width / this._numXPoints;
            var ptX = (pt.x - this._data.minX) * (diffTwoXPoints / this._data.diffPerPointX) + this._xStart;
            //Calculate y position
            var diffTwoYPoints = this._height / this._numYPoints;
            var ptY = (this._yStart + this._height) - (pt.y - this._data.minY) * (diffTwoYPoints / this._data.diffPerPointY);

            //Render dot
            if (pt.isDotShow) {
                //Drew White circle                
                var radgrad = this._context.createRadialGradient(ptX, ptY, 8, ptX - 5, ptY - 5, 0);
                radgrad.addColorStop(0, 'White');
                this._context.beginPath();
                this._context.fillStyle = radgrad;
                this._context.arc(ptX, ptY, 8, 0, 2 * Math.PI, false)
                this._context.fill();
                this._context.lineWidth = 1;
                this._context.strokeStyle = '#ecf0f1';
                this._context.stroke();

                //Drew inner circle   
                //radgrad = _context.createRadialGradient(ptX-3, ptY-3, 8-3, ptX - 5, ptY - 5, 0);
                radgrad.addColorStop(0, pt.dotColor);
                this._context.beginPath();
                this._context.fillStyle = radgrad;
                this._context.arc(ptX, ptY, 2, 0, 2 * Math.PI, false)
                this._context.fill();
                this._context.lineWidth = 1;
                this._context.strokeStyle = pt.dotColor;
                this._context.stroke();

                this._context.closePath();
            }
            this._prevX = ptX;
            this._prevY = ptY;
        }
    }

    drawLine(startX, startY, endX, endY, color, lineWidth, lineDash) {
        if (color != null) this._context.strokeStyle = color;
        if (lineWidth != null) this._context.lineWidth = lineWidth;
        this._context.beginPath();
        this._context.setLineDash(lineDash); // Dashed/Solid line        
        this._context.moveTo(startX, startY);
        this._context.lineTo(endX, endY);
        this._context.stroke();
        this._context.closePath();
    }

    getLongestValueWidth() {
        this._context.font = this._font;

        var longestValueWidth = 0;
        for (var n = 0; n <= this._numYPoints; n++) {
            var value = this._data.maxY - (n * this._data.diffPerPointY);
            longestValueWidth = Math.max(longestValueWidth, this._context.measureText(value).width);
        }
        return longestValueWidth;
    }

    getSliderContent(age) {
        age = age + 35;
        var html = 'Retirment Age<br/>'
            + '<img src="data:image/svg+xml;utf8;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pgo8IS0tIEdlbmVyYXRvcjogQWRvYmUgSWxsdXN0cmF0b3IgMTkuMC4wLCBTVkcgRXhwb3J0IFBsdWctSW4gLiBTVkcgVmVyc2lvbjogNi4wMCBCdWlsZCAwKSAgLS0+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiBpZD0iQ2FwYV8xIiB4PSIwcHgiIHk9IjBweCIgdmlld0JveD0iMCAwIDQ5MCA0OTAiIHN0eWxlPSJlbmFibGUtYmFja2dyb3VuZDpuZXcgMCAwIDQ5MCA0OTA7IiB4bWw6c3BhY2U9InByZXNlcnZlIiB3aWR0aD0iMTZweCIgaGVpZ2h0PSIxNnB4Ij4KPGc+Cgk8Zz4KCQk8cGF0aCBkPSJNNDc0LjQ1OSwwdjQ5MEwxNS41NDEsMjQ0Ljk5MUw0NzQuNDU5LDB6IiBmaWxsPSIjMDAwMDAwIi8+Cgk8L2c+Cgk8Zz4KCTwvZz4KCTxnPgoJPC9nPgoJPGc+Cgk8L2c+Cgk8Zz4KCTwvZz4KCTxnPgoJPC9nPgoJPGc+Cgk8L2c+Cgk8Zz4KCTwvZz4KCTxnPgoJPC9nPgoJPGc+Cgk8L2c+Cgk8Zz4KCTwvZz4KCTxnPgoJPC9nPgoJPGc+Cgk8L2c+Cgk8Zz4KCTwvZz4KCTxnPgoJPC9nPgoJPGc+Cgk8L2c+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPGc+CjwvZz4KPC9zdmc+Cg==" />'
            + '<span class="waii-chart-slider-yrs"> ' + age + ' yrs </span>'
            + '<img src="data:image/svg+xml;utf8;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iaXNvLTg4NTktMSI/Pgo8IS0tIEdlbmVyYXRvcjogQWRvYmUgSWxsdXN0cmF0b3IgMTkuMC4wLCBTVkcgRXhwb3J0IFBsdWctSW4gLiBTVkcgVmVyc2lvbjogNi4wMCBCdWlsZCAwKSAgLS0+CjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmVyc2lvbj0iMS4xIiBpZD0iQ2FwYV8xIiB4PSIwcHgiIHk9IjBweCIgdmlld0JveD0iMCAwIDQ5MCA0OTAiIHN0eWxlPSJlbmFibGUtYmFja2dyb3VuZDpuZXcgMCAwIDQ5MCA0OTA7IiB4bWw6c3BhY2U9InByZXNlcnZlIiB3aWR0aD0iMTZweCIgaGVpZ2h0PSIxNnB4Ij4KPGc+Cgk8Zz4KCQk8cGF0aCBkPSJNMTUuNTQxLDQ5MFYwbDQ1OC45MTcsMjQ1LjAwOUwxNS41NDEsNDkweiIgZmlsbD0iIzAwMDAwMCIvPgoJPC9nPgoJPGc+Cgk8L2c+Cgk8Zz4KCTwvZz4KCTxnPgoJPC9nPgoJPGc+Cgk8L2c+Cgk8Zz4KCTwvZz4KCTxnPgoJPC9nPgoJPGc+Cgk8L2c+Cgk8Zz4KCTwvZz4KCTxnPgoJPC9nPgoJPGc+Cgk8L2c+Cgk8Zz4KCTwvZz4KCTxnPgoJPC9nPgoJPGc+Cgk8L2c+Cgk8Zz4KCTwvZz4KCTxnPgoJPC9nPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+CjxnPgo8L2c+Cjwvc3ZnPgo=" />';
        return html;
    }

    renderCanvasChart() {
        let retirementAges = this.data.retirementAges;
        let likRetSums = this.data.likRetSums;
        let likRetLegacies = this.data.likRetLegacies;
        let max = parseInt(Math.max(...likRetSums).toFixed(0));
        let min = parseInt(Math.min(...likRetSums).toFixed(0));

        this.minSLiderVal = retirementAges[0];
        this.maxSLiderVal = retirementAges[retirementAges.length - 1];

        // $(".waii-chart-slider-container").css({
        //     position: "absolute",
        //     top: this._yStart + this._height,
        //     left: this._xStart,
        //     "max-width": this._width
        // });
        // this.desiredRetirementAge = parseInt(this.user.desiredRetirementAge);

        let dataDef = {
            title: "",
            xLabel: '',
            yLabel: '',
            labelFont: '19pt Arial',
            dataPointFont: '10pt Arial',
            minX: retirementAges[0],
            maxX: retirementAges[retirementAges.length - 1],
            diffPerPointX: 5,
            minY: min,
            maxY: max,
            diffPerPointY: ((max - min) / 7).toFixed(0),
            dataPoints: [
                [{
                    x: this.data.currentAge,
                    y: 0,
                    lineType: this._lineType.solid,
                    color: "#34495e",
                    isDotShow: false
                }, {
                    x: parseFloat(this.user.desiredRetirementAge),
                    y: likRetSums[retirementAges.findIndex(item => item == parseFloat(this.user.desiredRetirementAge))],
                    lineType: this._lineType.solid,
                    color: "#34495e",
                    isDotShow: false
                }, {
                    x: this.data.lifeExpectancy,
                    y: likRetLegacies[retirementAges.findIndex(item => item == parseFloat(this.user.desiredRetirementAge))],
                    lineType: this._lineType.solid,
                    color: "#34495e",
                    isDotShow: false
                }],
                [{
                    x: this.data.currentAge,
                    y: 0,
                    lineType: this._lineType.solid,
                    color: "#e74c3c",
                    isDotShow: false
                }, {
                    x: parseFloat(this.user.desiredRetirementAge),
                    y: parseFloat(this.data.desRetSum).toFixed(),
                    lineType: this._lineType.solid,
                    color: "#e74c3c",
                    isDotShow: false
                }, {
                    x: this.data.lifeExpectancy,
                    y: parseFloat(this.data.desiredLegacyAmount).toFixed(0),
                    lineType: this._lineType.dashed,
                    color: "#e74c3c",
                    isDotShow: false
                }],
                [{
                    x: parseFloat(this.user.desiredRetirementAge),
                    y: likRetSums[0],
                    lineType: this._lineType.dashed,
                    color: "#95a5a6",
                    isDotShow: false
                }, {
                    x: parseFloat(this.user.desiredRetirementAge),
                    y: likRetLegacies[retirementAges.findIndex(item => item == parseFloat(this.user.desiredRetirementAge))],
                    lineType: this._lineType.dashed,
                    color: "#95a5a6", isDotShow: true,
                    dotColor: "#e74c3c"
                }, {
                    x: parseFloat(this.user.desiredRetirementAge),
                    y: likRetSums[retirementAges.findIndex(item => item == parseFloat(this.user.desiredRetirementAge))],
                    lineType: this._lineType.dashed,
                    color: "#95a5a6",
                    isDotShow: true,
                    dotColor: "#34495e"
                }]
            ],
        };
        this.render('canvas', dataDef);
    }

    generateSlider() {
        debugger;
        var self = this;
        var rangeSlider = function () {
            var slider = $('.range-slider'),
                range = $('.range-slider__range')
            // thumb = $('.range-slider__range::-webkit-slider-thumb')

            // let div = document.createElement('div');
            // thumb.append(div);
            // div.classList.add("divSlider");
            // div.style.height = "50px";
            // div.style.width = "50px";
            // div.style.backgroundColor = "#000";
            let value = $('.slider-value');
            value.html(self.user.desiredRetirementAge);

            slider.each(function () {

                // var el, newPoint, newPlace, offset, width;

                value.each(function () {
                    var value = $(this).prev().attr('value');
                    $(this).html(value);
                });

                range.on('input', function () {
                    $(this).next(value).html(this.value);
                    self.user.desiredRetirementAge = this.value;
                    self.renderCanvasChart();

                    // el = $(".slider-value");

                    // // Measure width of range input
                    // width = el.width();

                    // // Figure out placement percentage between left and right of input
                    // newPoint = (el.val() - el.attr("min")) / (el.attr("max") - el.attr("min"));

                    // // Janky value to get pointer to line up better
                    // offset = -1.3;

                    // // Prevent bubble from going beyond left or right (unsupported browsers)
                    // if (newPoint < 0) { newPlace = 0; }
                    // else if (newPoint > 1) { newPlace = width; }
                    // else { newPlace = width * newPoint + offset; offset -= newPoint; }

                    // // Move bubble
                    // el
                    //     .next(value)
                    //     .css({
                    //         left: newPlace,
                    //         marginLeft: offset + "%"
                    //     })
                    //     .text(el.val());
                });
            });

            // var handle = $("#custom-handle");
            // $("#slider").slider({
            //     create: function () {
            //         //handle.text($(this).slider("value"));  
            //         handle.html(self.getSliderContent($(this).slider("value")));
            //     },
            //     slide: function (event, ui) {
            //         //handle.text(ui.value);
            //         handle.html(self.getSliderContent(ui.value));
            //     }
            // });
        };

        rangeSlider();
    }
}

