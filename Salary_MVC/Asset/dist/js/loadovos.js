/**   
* 脚本名称：页面加载Loading脚本    
* by 知乎：小萧ovo 
* 脚本引入时，脚本应放到body内，否则报错；
* 使用方法：ovoload.loading.add("加载中，请稍候"); //可带文字可不带文字  默认一样式直接ovoload.loading.add()
* 删除方法：  ovoload.loading.remove();
*/
var ovoload = window.whir || {};
ovoload.loading =
    {
        add: function (title) {
            function createStyle() {
                return "body{font-family:Microsoft YaHei;}span.ovoloadsp {color:#fff;font-size: 26px;font-weight: bold;position: absolute;top: 50%;left: 50%;margin-top: 50px;}.loading-mask {opacity:0;position: fixed;z-index: 2000; background-color: rgba(0, 0, 0, 0.7); background-color: hsla(0, 0%, 0%, .7);_background:#000;*background:#000;#background:#000; background:#000\\0;margin: 0;top: 0;right: 0;bottom: 0;left: 0;transition: opacity .3s}.ajax-spinner-bars{position:absolute;width:35px;height:35px;left:50%;top:50%}.ajax-spinner-bars>div{position:absolute;width:5px;height:18px;background-color:#fff;opacity:.05;animation:ovofadeit .8s linear infinite}.ajax-spinner-bars>.ovobar-1{transform:rotate(0deg) translate(0,-25px);animation-delay:.05s}.ajax-spinner-bars>.ovobar-2{transform:rotate(22.5deg) translate(0,-25px);animation-delay:.1s}.ajax-spinner-bars>.ovobar-3{transform:rotate(45deg) translate(0,-25px);animation-delay:.15s}.ajax-spinner-bars>.ovobar-4{transform:rotate(67.5deg) translate(0,-25px);animation-delay:.2s}.ajax-spinner-bars>.ovobar-5{transform:rotate(90deg) translate(0,-25px);animation-delay:.25s}.ajax-spinner-bars>.ovobar-6{transform:rotate(112.5deg) translate(0,-25px);animation-delay:.3s}.ajax-spinner-bars>.ovobar-7{transform:rotate(135deg) translate(0,-25px);animation-delay:.35s}.ajax-spinner-bars>.ovobar-8{transform:rotate(157.5deg) translate(0,-25px);animation-delay:.4s}.ajax-spinner-bars>.ovobar-9{transform:rotate(180deg) translate(0,-25px);animation-delay:.45s}.ajax-spinner-bars>.ovobar-10{transform:rotate(202.5deg) translate(0,-25px);animation-delay:.5s}.ajax-spinner-bars>.ovobar-11{transform:rotate(225deg) translate(0,-25px);animation-delay:.55s}.ajax-spinner-bars>.ovobar-12{transform:rotate(247.5deg) translate(0,-25px);animation-delay:.6s}.ajax-spinner-bars>.ovobar-13{transform:rotate(270deg) translate(0,-25px);animation-delay:.65s}.ajax-spinner-bars>.ovobar-14{transform:rotate(292.5deg) translate(0,-25px);animation-delay:.7s}.ajax-spinner-bars>.ovobar-15{transform:rotate(315deg) translate(0,-25px);animation-delay:.75s}.ajax-spinner-bars>.ovobar-16{transform:rotate(337.5deg) translate(0,-25px);animation-delay:.8s}@keyframes ovofadeit{0%{opacity:1}100%{opacity:0}}";
            }
            function addStyle() {
                var code = "body{font-family:Microsoft YaHei;}span.ovoloadsp {color:#fff;font-size: 26px;font-weight: bold;position: absolute;top: 50%;left: 50%;margin-top: 50px;}.loading-mask {opacity:0;position: fixed;z-index: 2000;background-color: rgba(0, 0, 0, 0.7); background-color: hsla(0, 0%, 0%, .7);_background:#000;*background:#000;#background:#000; background:#000\\0; margin: 0;top: 0;right: 0;bottom: 0;left: 0;transition: opacity .3s}.ajax-spinner-bars{position:absolute;width:35px;height:35px;left:50%;top:50%}.ajax-spinner-bars>div{position:absolute;width:5px;height:18px;background-color:#fff;opacity:.05;animation:ovofadeit .8s linear infinite}.ajax-spinner-bars>.ovobar-1{transform:rotate(0deg) translate(0,-25px);animation-delay:.05s}.ajax-spinner-bars>.ovobar-2{transform:rotate(22.5deg) translate(0,-25px);animation-delay:.1s}.ajax-spinner-bars>.ovobar-3{transform:rotate(45deg) translate(0,-25px);animation-delay:.15s}.ajax-spinner-bars>.ovobar-4{transform:rotate(67.5deg) translate(0,-25px);animation-delay:.2s}.ajax-spinner-bars>.ovobar-5{transform:rotate(90deg) translate(0,-25px);animation-delay:.25s}.ajax-spinner-bars>.ovobar-6{transform:rotate(112.5deg) translate(0,-25px);animation-delay:.3s}.ajax-spinner-bars>.ovobar-7{transform:rotate(135deg) translate(0,-25px);animation-delay:.35s}.ajax-spinner-bars>.ovobar-8{transform:rotate(157.5deg) translate(0,-25px);animation-delay:.4s}.ajax-spinner-bars>.ovobar-9{transform:rotate(180deg) translate(0,-25px);animation-delay:.45s}.ajax-spinner-bars>.ovobar-10{transform:rotate(202.5deg) translate(0,-25px);animation-delay:.5s}.ajax-spinner-bars>.ovobar-11{transform:rotate(225deg) translate(0,-25px);animation-delay:.55s}.ajax-spinner-bars>.ovobar-12{transform:rotate(247.5deg) translate(0,-25px);animation-delay:.6s}.ajax-spinner-bars>.ovobar-13{transform:rotate(270deg) translate(0,-25px);animation-delay:.65s}.ajax-spinner-bars>.ovobar-14{transform:rotate(292.5deg) translate(0,-25px);animation-delay:.7s}.ajax-spinner-bars>.ovobar-15{transform:rotate(315deg) translate(0,-25px);animation-delay:.75s}.ajax-spinner-bars>.ovobar-16{transform:rotate(337.5deg) translate(0,-25px);animation-delay:.8s}@keyframes ovofadeit{0%{opacity:1}100%{opacity:0}}";
                if (navigator && navigator.userAgent.match(/msie/i)) {
                    function loadCssCode(){
                        var style = document.createElement('style');
                            style.type = 'text/css';
                            style.rel = 'stylesheet';
                            try{
                                //for Chrome Firefox Opera Safari
                                style .appendChild(document.createTextNode(code));
                            }catch(ex){
                                //for IE
                                style.styleSheet.cssText = code;
                            }
                            var head = document.getElementsByTagName('head')[0];
                            head.appendChild(style);
                        }
                    loadCssCode('body{background-color:#f00}');
                  } else {
                    var style = document.createElement("style");
                    style.type = "text/css";
                    style.innerHTML = code;
                    window.document.body.appendChild(style);
                  }
            }
            addStyle();// 页面加入样式
            var opacity = 0;
            var arr = this.getPageSize();
            var width = parseInt(arr[2]);
            var height = parseInt(arr[3]);
            //背景遮罩    
            var mask = document.createElement("div");
            mask.id = 'mask';
            mask.className = 'loading-mask amt_load';
            var Event = {};
            Event.addEvents = function(target,eventType,handle){
                if(document.addEventListener){
                    Event.addEvents = function(target,eventType,handle){
                        target.addEventListener(eventType,handle,false);
                    };
                }else{
                    Event.addEvents = function(target,eventType,handle){
                        target.attachEvent('on'+eventType,function(){
                            handle.call(target,arguments);
                        });
                    };
                };
                Event.addEvents(target,eventType,handle);
            };
            Event.addEvents(mask,'touchstart',function (e) { e.preventDefault(); });
            Event.addEvents(mask,'touchmove',function (e) { e.preventDefault(); });
            Event.addEvents(mask,'touchend',function (e) { e.preventDefault(); });
            // mask.addEventListener('touchstart', function (e) { e.preventDefault(); }, false);   //触摸事件    
            // mask.addEventListener('touchmove', function (e) { e.preventDefault(); }, false);    //滑动事件    
            // mask.addEventListener('touchend', function (e) { e.preventDefault(); }, false);     //离开元素事件    
            document.body.appendChild(mask);
            var loading = document.createElement("div");
            loading.id = 'ovo-ajax-spinner-bars';
            loading.className = 'ajax-spinner-bars';
            loading.innerHTML = '<div class="ovobar-1"></div><div class="ovobar-2"></div><div class="ovobar-3"></div><div class="ovobar-4"></div><div class="ovobar-5"></div><div class="ovobar-6"></div><div class="ovobar-7"></div><div class="ovobar-8"></div><div class="ovobar-9"></div><div class="ovobar-10"></div><div class="ovobar-11"></div><div class="ovobar-12"></div><div class="ovobar-13"></div><div class="ovobar-14"></div><div class="ovobar-15"></div><div class="ovobar-16"></div>';
            document.getElementById('mask').appendChild(loading);
            title = (title != undefined && title.length > 0) ? title : "";
            if (title != "") {
                var tits = document.createElement("span");
                tits.id = 'ovoloadsp';
                tits.className = 'ovoloadsp';
                tits.innerHTML = title;
                document.getElementById('mask').appendChild(tits);
                var titw = document.getElementById('ovoloadsp').offsetWidth; //获取文字宽度
                document.getElementById('ovoloadsp').style.marginLeft = '-' + (titw / 2 - 4) + "px";
            }
            //先隐藏0.5s，之后再展示
            setTimeout(function () { 
                mask.style.opacity = 1; 
            }, 500);
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
    ovoload.loading.add(info)
}
function hideLoad() {
    ovoload.loading.remove();
}