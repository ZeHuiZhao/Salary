/* form_bow begin */
// 切换主标题的tab
//$('body').on('click', '.ov-tab a', function () {
//    $(this).addClass('sel').siblings().removeClass('sel')
//    var tabName = $(this).data('tab'); //渠道名字

//    //ajax 切换表格数据

//    return false
//})
//生成筛选导航 模拟ajax加载 begin
var preview_num = $(".ov-preview li").length;
var preview_html = '';
for (var i = 0; i < preview_num; i++) {
    var dataId = $(".ov-preview li").eq(i).data('id');
    var dataSpan = $(".ov-preview li").eq(i).find('.preview_span').html();
    var dataActive = $(".ov-preview li").eq(i).find('.active').data('name');
    preview_html += '<a data-id="' + dataId + '">' + '<i>' + dataSpan + ':</i>' + '<em>' + dataActive + '</em>' + '</a>'
}
$(".ov-choice").append(preview_html);
//生成筛选导航 模拟ajax加载 end

//获取筛选条件：
$('body').on('click', '.ov-nav a', function () {
    var _this = $(this);
    _this.addClass('active').siblings().removeClass('active')
    var navName = _this.data("name");
    var active = _this.parents('li').data("id");
    $(".ov-choice a").each(function () {
        if ($(this).data("id") == active) {
            $(this).find('em').html(navName);
        }
    });
})
// 关闭筛选条件
$('.ov-select').on('click', 'a', function () {
    $(this).hide().siblings().css({ "display": "block" });
    $(".ov-choice").slideToggle();
    $(".ov-preview").slideToggle();
})
//搜索条件切换
function SOGButtonControl_JsInit(str_id) {
    $('.ul_ddl_li').click(function () {
        $('#btn_screen').html($(this).attr('text'));
    });
};
SOGButtonControl_JsInit('ul_ddl_field_search');
/* form_bow end */