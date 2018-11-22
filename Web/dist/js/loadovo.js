/**   
* 脚本名称：页面加载Loading脚本    
* by 知乎：小萧ovo 
* 脚本引入时，脚本应放到body内，否则报错；
* 共四套样式 赋值：1 2 3 4
* 使用方法：ovoload.loading.add(1,"加载中，请稍候"); //可带文字可不带文字  默认一样式直接ovoload.loading.add()
* 删除方法：  ovoload.loading.remove();
*/
var ovoload = window.whir || {};
ovoload.loading =
    {
        add: function (city, title) {
            city = city == undefined ? 1 : city;
            function createStyle() {
                if (city == '1') {// loading 样式一 默认显示这个
                    return ".loadsp{position: relative;top: -14px;left: 11px;font-size: 14px;color: #6d6969;}.loading-mask {position: absolute;z-index: 2000;background-color: hsla(0, 0%, 100%, .9);margin: 0;top: 0;right: 0;bottom: 0;left: 0;transition: opacity .3s}.loading-mask.is-fullscreen {position: fixed}.loading-mask.is-fullscreen .loading-spinner {margin-top: -25px}.loading-mask.is-fullscreen .loading-spinner .circulars {height: 50px;width: 50px}.loading-spinner {top: 50%;margin-top: -21px; width: 100%;text-align: center;position: absolute}.loading-spinner .loading-text {color: #409eff;margin: 3px 0;font-size: 14px}.loading-spinner .circulars {height: 42px;width: 42px;-webkit-animation:loading-rotate 2s linear infinite;animation: loading-rotate 2s linear infinite}.loading-spinner .paths {-webkit-animation: loading-dash 1.5s ease-in-out infinite;animation: loading-dash 1.5s ease-in-out infinite;stroke-dasharray: 90, 150;stroke-dashoffset: 0;stroke-width: 2;stroke: #409eff;stroke-linecap: round}.loading-spinner i {color: #409eff}.loading-fade-enter,.loading-fade-leave-active {opacity: 0}@keyframes loading-rotate {to {transform: rotate(1turn)}}@keyframes loading-dash {0% {stroke-dasharray: 1, 200;stroke-dashoffset: 0}50% {stroke-dasharray: 90, 150;stroke-dashoffset: -40px}to {stroke-dasharray: 90, 150;stroke-dashoffset: -120px}}"
                } else if (city == '2') {// loading 样式二 
                    return "#loading-tit{position: absolute;width: auto;height: 50px;line-height: 50px;display: inline-block;font-size: 0.5em;text-align: center;font-family: initial;z-index: 100001;color: rgb(68, 68, 68);}.loading-mask {position: absolute;z-index: 2000;background-color: hsla(0, 0%, 100%, .9);margin: 0;top: 0;right: 0;bottom: 0;left: 0;transition: opacity .3s}.amt_load{ transition: all 1s;-moz-transition: all 1s;-webkit-transition: all 1s;-o-transition: all 1s;}.droploading{display: inline-block;position: absolute;top: 5px;left: -38px;height: 22px;width: 22px;border-radius: 100%;margin: 6px;border: 2px solid #409eff;border-bottom-color: transparent;vertical-align: middle;-webkit-animation: rotate 0.75s linear infinite;animation: rotate 0.75s linear infinite;}@-webkit-keyframes rotate {0% {-webkit-transform: rotate(0deg);}50% {-webkit-transform: rotate(180deg);}100% {-webkit-transform: rotate(360deg);}}@keyframes rotate {0% {transform: rotate(0deg);}50% {transform: rotate(180deg);}100% {transform: rotate(360deg);}}"
                } else if (city == '3') {// loading 样式三
                    return ".loadsp{color: #6d6969; -webkit-animation:loadspcolor 6s ease-in-out infinite; animation: loadspcolor 6s ease-in-out infinite; position: relative;top: -14px;left: 11px;font-size: 14px;}.loading-mask {position: absolute;z-index: 2000;background-color: hsla(0, 0%, 100%, .9);margin: 0;top: 0;right: 0;bottom: 0;left: 0;transition: opacity .3s}.loading-mask.is-fullscreen {position: fixed}.loading-mask.is-fullscreen .loading-spinner {margin-top: -25px}.loading-mask.is-fullscreen .loading-spinner .circulars {height: 50px;width: 50px}.loading-spinner {top: 50%;margin-top: -21px; width: 100%;text-align: center;position: absolute}.loading-spinner .loading-text {color: #409eff;margin: 3px 0;font-size: 14px}.loading-spinner .circulars {height: 42px;width: 42px;-webkit-animation:loading-rotate 2s linear infinite;animation: loading-rotate 2s linear infinite}.loading-spinner .paths {-webkit-animation: loading-dash 1.5s ease-in-out infinite, color 6s ease-in-out infinite; animation: loading-dash 1.5s ease-in-out infinite, color 6s ease-in-out infinite;stroke-dasharray: 90, 150;stroke-dashoffset: 0;stroke-width: 2;stroke: #409eff;stroke-linecap: round}.loading-spinner i {color: #409eff}.loading-fade-enter,.loading-fade-leave-active {opacity: 0}@keyframes loading-rotate {to {transform: rotate(1turn)}}@keyframes loading-dash {0% {stroke-dasharray: 1, 200;stroke-dashoffset: 0}50% {stroke-dasharray: 90, 150;stroke-dashoffset: -40px}to {stroke-dasharray: 90, 150;stroke-dashoffset: -120px}}@-webkit-keyframes color {100%,0% {stroke: #d62d20;}40% {stroke: #0057e7;}66% {stroke: #008744;}80%,90% {stroke: #ffa700;}} @keyframes color {100%,0% {stroke: #d62d20;}40% {stroke: #0057e7;}66% {stroke: #008744; }80%,90% {stroke: #ffa700;}}@-webkit-keyframes loadspcolor {100%,0% {stroke: #d62d20;}40% {color: #0057e7;}66% {color: #008744;}80%,90% {color: #ffa700;}} @keyframes loadspcolor {100%,0% {color: #d62d20;}40% {color: #0057e7;}66% {color: #008744; }80%,90% {color: #ffa700;}}"
                } else if (city == '4') {// loading 样式四 苹果风格
                    return "span.ovoloadsp {color:#939ea5;font-size: 14px;position: absolute;top: 50%;left: 50%;margin-top: 35px;}.loading-mask {position: absolute;z-index: 2000;background-color: hsla(0, 0%, 100%, .9);margin: 0;top: 0;right: 0;bottom: 0;left: 0;transition: opacity .3s}.ajax-spinner-bars{position:absolute;width:35px;height:35px;left:50%;top:50%}.ajax-spinner-bars>div{position:absolute;width:2px;height:8px;background-color:#25363f;opacity:.05;animation:ovofadeit .8s linear infinite}.ajax-spinner-bars>.ovobar-1{transform:rotate(0deg) translate(0,-12px);animation-delay:.05s}.ajax-spinner-bars>.ovobar-2{transform:rotate(22.5deg) translate(0,-12px);animation-delay:.1s}.ajax-spinner-bars>.ovobar-3{transform:rotate(45deg) translate(0,-12px);animation-delay:.15s}.ajax-spinner-bars>.ovobar-4{transform:rotate(67.5deg) translate(0,-12px);animation-delay:.2s}.ajax-spinner-bars>.ovobar-5{transform:rotate(90deg) translate(0,-12px);animation-delay:.25s}.ajax-spinner-bars>.ovobar-6{transform:rotate(112.5deg) translate(0,-12px);animation-delay:.3s}.ajax-spinner-bars>.ovobar-7{transform:rotate(135deg) translate(0,-12px);animation-delay:.35s}.ajax-spinner-bars>.ovobar-8{transform:rotate(157.5deg) translate(0,-12px);animation-delay:.4s}.ajax-spinner-bars>.ovobar-9{transform:rotate(180deg) translate(0,-12px);animation-delay:.45s}.ajax-spinner-bars>.ovobar-10{transform:rotate(202.5deg) translate(0,-12px);animation-delay:.5s}.ajax-spinner-bars>.ovobar-11{transform:rotate(225deg) translate(0,-12px);animation-delay:.55s}.ajax-spinner-bars>.ovobar-12{transform:rotate(247.5deg) translate(0,-12px);animation-delay:.6s}.ajax-spinner-bars>.ovobar-13{transform:rotate(270deg) translate(0,-12px);animation-delay:.65s}.ajax-spinner-bars>.ovobar-14{transform:rotate(292.5deg) translate(0,-12px);animation-delay:.7s}.ajax-spinner-bars>.ovobar-15{transform:rotate(315deg) translate(0,-12px);animation-delay:.75s}.ajax-spinner-bars>.ovobar-16{transform:rotate(337.5deg) translate(0,-12px);animation-delay:.8s}@keyframes ovofadeit{0%{opacity:1}100%{opacity:0}}"
                }
            }
            function addStyle() {
                var style = document.createElement("style");
                style.type = "text/css";
                style.innerHTML = createStyle();
                window.document.head.appendChild(style);
            }
            addStyle();// 页面加入样式
            // opacity = opacity == undefined ? 1 : opacity;
            var arr = this.getPageSize();
            var width = parseInt(arr[2]);
            var height = parseInt(arr[3]);
            //背景遮罩    
            var mask = document.createElement("div");
            mask.id = 'mask';
            mask.className = 'loading-mask amt_load';
            mask.addEventListener('touchstart', function (e) { e.preventDefault(); }, false);   //触摸事件    
            mask.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);    //滑动事件    
            mask.addEventListener('touchend', function (e) { e.preventDefault(); }, false);     //离开元素事件    
            document.body.appendChild(mask);
            function ovoElement() { //模拟Element的loading
                title = (title != undefined && title.length > 0) ? title : "";
                var loading = document.createElement("div");
                loading.id = 'loading-spinner';
                loading.className = 'loading-spinner';
                document.getElementById('mask').appendChild(loading);
                function parseSVG(s) {
                    var divs = document.createElementNS('http://www.w3.org/1999/xhtml', 'div');
                    divs.innerHTML = '<svg id="loading-svg" class="circulars" viewBox="25 25 50 50"   xmlns="http://www.w3.org/2000/svg">' + s + '</svg>';
                    var frag = document.createDocumentFragment();
                    while (divs.firstChild.firstChild)
                        frag.appendChild(divs.firstChild.firstChild);
                    return frag;
                }
                document.getElementById('loading-spinner').appendChild(parseSVG('<svg id="loading-svg" viewBox="25 25 50 50" class="circulars"  xmlns="http://www.w3.org/2000/svg"><circle cx="50" cy="50" r="20" fill="none" class="paths"></circle></svg>'));
                if (title != "") {
                    // var tits = document.createElement("div");
                    // tits.innerHTML = '<span>'+title+'</span>';
                    var tits = document.createElement("span");
                    tits.className = 'loadsp';
                    tits.innerHTML = title;
                    document.getElementById('loading-spinner').appendChild(tits);
                }
            }
            if (city == '1') {//loading 默认一
                ovoElement();
            } else if (city == '2') {//loading 样式 二
                var loading = document.createElement("div");
                loading.id = 'loading-tit';
                loading.style.left = (width / 2 - 50) + "px";
                loading.style.top = (height / 2 - 50) + "px";
                title = (title != undefined && title.length > 0) ? title : "";
                loading.innerHTML = title;
                document.getElementById('mask').appendChild(loading);
                var loadings = document.createElement("div");
                loadings.id = 'droploading';
                loadings.className = 'droploading';
                if (title == "") {
                    loadings.style.left = "0px";
                }
                document.getElementById('loading-tit').appendChild(loadings);
            } else if (city == '3') {//loading 样式 三
                ovoElement();
            } else if (city == '4') {//loading 样式 四
                var loading = document.createElement("div");
                loading.id = 'ovo-ajax-spinner-bars';
                loading.className = 'ajax-spinner-bars';
                loading.innerHTML = '<div class="ovobar-1"></div><div class="ovobar-2"></div><div class="ovobar-3"></div><div class="ovobar-4"></div><div class="ovobar-5"></div><div class="ovobar-6"></div><div class="ovobar-7"></div><div class="ovobar-8"></div><div class="ovobar-9"></div><div class="ovobar-10"></div><div class="ovobar-11"></div><div class="ovobar-12"></div><div class="ovobar-13"></div><div class="ovobar-14"></div><div class="ovobar-15"></div><div class="ovobar-16"></div>';
                document.getElementById('mask').appendChild(loading);
                if (title != "") {
                    var tits = document.createElement("span");
                    tits.id = 'ovoloadsp';
                    tits.className = 'ovoloadsp';
                    tits.innerHTML = title;
                    document.getElementById('mask').appendChild(tits);
                    var titw = document.getElementById('ovoloadsp').offsetWidth; //获取文字宽度
                    document.getElementById('ovoloadsp').style.marginLeft = '-' + (titw / 2 - 4) + "px";
                }
            }
            //先隐藏0.5s，之后再展示
            //mask.style.opacity = 0; setTimeout(function () { mask.style.opacity = 1; }, 500);
        },
        remove: function () {
            var element = document.getElementById("mask");
            element.parentNode.removeChild(element);
        },
        getPageSize: function () {
            var xScroll, yScroll;
            if (window.innerHeight && window.scrollMaxY) {
                xScroll = window.innerWidth + window.scrollMaxX;
                yScroll = window.innerHeight + window.scrollMaxY;
            } else {
                if (document.body.scrollHeight > document.body.offsetHeight) { // all but Explorer Mac        
                    xScroll = document.body.scrollWidth;
                    yScroll = document.body.scrollHeight;
                } else { // Explorer Mac...would also work in Explorer 6 Strict, Mozilla and Safari        
                    xScroll = document.body.offsetWidth;
                    yScroll = document.body.offsetHeight;
                }
            }
            var windowWidth = 0;
            var windowHeight = 0;
            var pageHeight = 0;
            var pageWidth = 0;
            if (self.innerHeight) { // all except Explorer        
                if (document.documentElement.clientWidth) {
                    windowWidth = document.documentElement.clientWidth;
                } else {
                    windowWidth = self.innerWidth;
                }
                windowHeight = self.innerHeight;
            } else {
                if (document.documentElement && document.documentElement.clientHeight) { // Explorer 6 Strict Mode        
                    windowWidth = document.documentElement.clientWidth;
                    windowHeight = document.documentElement.clientHeight;
                } else {
                    if (document.body) { // other Explorers        
                        windowWidth = document.body.clientWidth;
                        windowHeight = document.body.clientHeight;
                    }
                }
            }
            // for small pages with total height less then height of the viewport        
            if (yScroll < windowHeight) {
                pageHeight = windowHeight;
            } else {
                pageHeight = yScroll;
            }
            // for small pages with total width less then width of the viewport        
            if (xScroll < windowWidth) {
                pageWidth = xScroll;
            } else {
                pageWidth = windowWidth;
            }
            var arrayPageSize = new Array(pageWidth, pageHeight, windowWidth, windowHeight);
            return arrayPageSize;
        }
    };
function showLoad(info) {
    ovoload.loading.add(1, info)
}
function hideLoad() {
    ovoload.loading.remove();
}