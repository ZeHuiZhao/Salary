/*注册页面表单校验*/
$(function(){
	var check = $("#check");//勾选
	var confirm = $('#reg_submit');//确定提交
	var sendCode = $("#sendCode");//获取验证码
    //获取短信验证码倒计时120秒
    var allTime = 120, time;
    //发送手机验证码
    sendCode.click(function(){
    	allTime = 120 ;
    	if(!(/^1([2356789])\d{9}$/.test($("#mobile-number").val()))){
            //通知
            noty({text: '请输入正确的手机号码',
        		type:'error',
        	});
            return false;
	    }
	    else{
			var data = {};
			data['phone'] = $('input[name="mobile"]').val();//手机号
            data['func'] = '1';//注册验证码 
	        $.ajax({
	        type: "post",
	        url: sms,
	        data:data,
	        dataType: "json",
	        success: function(data){
				console.log(data);
	        	if(data.code == 200){
            		noty({
	                	text:data.msg,
		                type:'success',
		            });
		            sendCode.val("剩余" + allTime + "秒").attr("disabled", "disabled");
                    time = setInterval(subTime, 1000);
                    sendCode.css("backgroundColor", "#cccccc");
                    return true;
            	}else{
            		noty({
	                	text:data.msg,
		                type:'error',
		            });
            	}
	        }
	     	});  	
	    }
    });

    //获取验证码倒计时处的回调函数
    function subTime(){
        allTime--;
        sendCode.val("剩余" + allTime + "秒");
        if(allTime < 0){
            clearInterval(time);
            sendCode.val("重新获取").removeAttr("disabled");
            sendCode.css("backgroundColor", "rgba(255,163,0,1)");
            allTime=120;
        }
    }

	//注册提交
	$(".agbel").click(function(){
        if(check.prop("checked")==true){
            $(".checkMsg").remove();
        } 
	});
	confirm.click(function(){
		
		if(check.prop("checked")!=true){
			$(".checkMsg").remove();
			var checkkName = check.prop("checked");
			if (checkkName !=true) {
				checkMsg = "<span class='checkMsg' style='color:red;font-size:14px;'>需要同意协议</span>";
				$(".agbel").after(checkMsg); 
				return false;
			} else {
				$(".checkMsg").remove();
			}
		} 
		/*验证多一回*/
		if ($("#reg-name").attr("name") == "username") {
			$(".nickMsg").remove();
			var nickName = $("#reg-name").val();
			var regName = /[\u4e00-\u9fa5]{2,6}/
			if (nickName == "") {
			  errMsg = "<span class='nickMsg' style='color:red;font-size:14px;'>昵称不能为空</span>";
			} else if (!regName.test(nickName)) {
			  errMsg = "<span class='nickMsg' style='color:red;font-size:14px;'>由2-6个汉字组成</span>";
			  $("#reg-name").after(errMsg); 
			return false;
			} else {
			  errMsg = "";
			  $(".nickMsg").remove();
			}
			
		}

		if ($("#nameid").attr("name") == "nameid") {
			$(".nameidMsg").remove();
			var nameid = $("#nameid").val();
			var regnameid = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/; 
			if (nameid == "") {
			  errMsg = "<span class='nameidMsg' style='color:red;font-size:14px;'>身份证不能为空</span>";
			  $("#nameid").after(errMsg);
			return false;
			} else if (!regnameid.test(nameid)) {
			  errMsg = "<span class='nameidMsg' style='color:red;font-size:14px;'>身份证格式出错！</span>";
			  $("#nameid").after(errMsg);
			return false;
			} else {
			  errMsg = "";
			  $(".nameidMsg").remove();
			}
		}

		if ($("#email").attr("name") == "email") {
			$(".emailMsg").remove();
			var email = $("#email").val();
			var regEmail = /^\w+@\w+((\.\w+)+)$/;
			if (email == "") {
			  errMsg = "<span class='emailMsg' style='color:red;font-size:14px;'>邮箱不能为空</span>";
			  $("#email").after(errMsg);
			return false;
			} else if (!regEmail.test(email)) {
			  errMsg = "<span class='emailMsg' style='color:red;font-size:14px;'>邮箱账号@域名。如123456@qq.com、abc@163.com</span>";
			  $("#email").after(errMsg);
			return false;
			} else {
			  errMsg = "";
			  $(".emailMsg").remove();
			}
		  }
		
		  if ($("#reg-ib").attr("name") == "group") {
			$(".groupMsg").remove();
			var group = $("#reg-ib").val();
			if (group == "") {
			  errMsg = "<span class='groupMsg' style='color:red;font-size:14px;'>所属经销商不能为空</span>";
			  $("#reg-ib").after(errMsg);
			return false;
			} else {
			  errMsg = "";
			  $(".groupMsg").remove();
			}
		}

		if ($("#password").attr("name") == "password") {
			$(".pwdMsg").remove();
			var pwd = $("#password").val();
			var regPwd = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^]{6,12}$/;
			if (pwd == "") {
			  errMsg = "<span class='pwdMsg' style='color:red;font-size:14px;'>密码不能为空</span>";
			  $("#password").after(errMsg);
			return false;
			} else if (!regPwd.test(pwd)) {
			  errMsg = "<span class='pwdMsg' style='color:red;font-size:14px;'>密码必须是6-12位且包含大小写字母和数字</span>";
			  $("#password").after(errMsg);
			return false;
			} else {
			  errMsg = "";
			  $(".pwdMsg").remove();
			}
		  }
		
		  if ($("#regpassword").attr("name") == "repassword") {
			$(".pwd2Msg").remove();
			var pwd2 = $("#regpassword").val();
			var pwd = $("#password").val();
			if (pwd2 == ""|| pwd2 != pwd) {
			  errMsg = "<span class='pwd2Msg' style='color:red;font-size:14px;'>两次输入密码不一致</span>";
			  $("#regpassword").after(errMsg);
			return false;
			} else {
			  errMsg = "";
			  $(".pwd2Msg").remove();
			}
		  }

		  if ($("#mobile-number").attr("name") == "mobile") {
			$(".phoneMsg").remove();
			var phone = $("#mobile-number").val();
			var regPhone = /^1[3456789]\d{9}$/;
			if (phone == "") {
			  errMsg = "<span class = 'phoneMsg' style = 'color:red;font-size:14px;' > 手机号不能为空 </span>"
			  $("#mobile-number").parent().after(errMsg);
			return false;
			} else if (!regPhone.test(phone)) {
			  errMsg = "<span class = 'phoneMsg' style = 'color:red;font-size:14px;' > 不是正确的手机号 </span>"
			  $("#mobile-number").parent().after(errMsg);
			return false;
			} else {
			  errMsg = "";
			  $(".phoneMsg").remove();
			}
		  } 

		  if ($("#phone-code").attr("name") == "mobile_verify") {
			$(".verifyMsg").remove();
			var group = $("#reg-ib").val();
			if (group == "") {
			  errMsg = "<span class='verifyMsg' style='color:red;font-size:14px;'>验证码不能为空</span>";
			  $(".fiv_code").after(errMsg);
			return false;
			} else {
			  errMsg = "";
			  $(".verifyMsg").remove();
			}
		}

		var data = {};
		data['phone'] = $('input[name="mobile"]').val();//手机号
		data['code'] = $('input[name="mobile_verify"]').val();//短信验证码
		data['platform'] = platform;//项目标识
		data['invite_code'] = $('input[name="group"]').val();//代理商 （mt4账号）
        data['real_name'] = $('input[name="username"]').val();//姓名
		data['identity_card'] = $('input[name="nameid"]').val();//身份证
		data['user_email'] = $('input[name="email"]').val();//邮箱
		data['password'] = $('input[name="password"]').val();//设置密码
		data['again_password'] = $('input[name="repassword"]').val();//确认密码
		
		$.ajax({
	        type: "post",
	        url: register,
	        data:data,
	        dataType: "json",
	        success: function(data){
				console.log(data);
	        	if(data.code == 200){
            		noty({
	                	text:data.msg,
		                type:'success',
								});
								var lochref = window.location.protocol+"//"+window.location.host+"/page/login/login.html";
								location.href =lochref;
            	}else{
            		noty({
	                	text:data.info,
		                type:'error',
		            });
            	}
	        }
	     });
	})


	/**/ 
	var errMsg;
	$.each($("input"), function (i, val) {
	  $(val).blur(function () {
		if ($(val).attr("name") == "username") {
		  $(".nickMsg").remove();
		  var nickName = val.value;
		  var regName = /[\u4e00-\u9fa5]{2,6}/
		  if (nickName == "" || nickName.trim() == "") {
			errMsg = "<span class='nickMsg' style='color:red;font-size:14px;'>昵称不能为空</span>";
		  } else if (!regName.test(nickName)) {
			errMsg = "<span class='nickMsg' style='color:red;font-size:14px;'>由2-6个汉字组成</span>";
		  } else {
			errMsg = "";
		  }
		  $(this).after(errMsg); 
		}  else if ($(val).attr("name") == "nameid") {
			$(".nameidMsg").remove();
			var nameid = val.value;
			var regnameid = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/; 
			if (nameid == "" || nameid.trim() == "") {
			  errMsg = "<span class='nameidMsg' style='color:red;font-size:14px;'>身份证不能为空</span>";
			} else if (!regnameid.test(nameid)) {
			  errMsg = "<span class='nameidMsg' style='color:red;font-size:14px;'>身份证格式出错！</span>";
			} else {
			  errMsg = "";
			}
			$(this).after(errMsg);
		} else if ($(val).attr("name") == "email") {
		  $(".emailMsg").remove();
		  var email = val.value;
		  var regEmail = /^\w+@\w+((\.\w+)+)$/;
		  if (email == "" || email.trim() == "") {
			errMsg = "<span class='emailMsg' style='color:red;font-size:14px;'>邮箱不能为空</span>";
		  } else if (!regEmail.test(email)) {
			errMsg = "<span class='emailMsg' style='color:red;font-size:14px;'>邮箱账号@域名。如123456@qq.com、abc@163.com</span>";
		  } else {
			errMsg = "";
		  }
		  $(this).after(errMsg);
		} else if ($(val).attr("name") == "group") {
			$(".groupMsg").remove();
			var group = val.value;
			if (group == "" || group.trim() == "") {
			  errMsg = "<span class='groupMsg' style='color:red;font-size:14px;'>所属经销商不能为空</span>";
			} else {
			  errMsg = "";
			}
			$(this).after(errMsg);
		} else if ($(val).attr("name") == "password") {
		  $(".pwdMsg").remove();
		  var pwd = val.value;
		  var regPwd = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[^]{6,12}$/;
		  if (pwd == "" || pwd.trim() == "") {
			errMsg = "<span class='pwdMsg' style='color:red;font-size:14px;'>密码不能为空</span>";
		  } else if (!regPwd.test(pwd)) {
			errMsg = "<span class='pwdMsg' style='color:red;font-size:14px;'>密码必须是6-12位且包含大小写字母和数字</span>";
		  } else {
			errMsg = "";
		  }
		  $(this).after(errMsg);
		} else if ($(val).attr("name") == "repassword") {
		  $(".pwd2Msg").remove();
		  var pwd2 = val.value;
		  var pwd = $("#password").val();
		  if (pwd2 == "" || pwd2.trim() == "" || pwd2 != pwd) {
			errMsg = "<span class='pwd2Msg' style='color:red;font-size:14px;'>两次输入密码不一致</span>";
		  } else {
			errMsg = "";
		  }
		  $(this).after(errMsg);
		} else if ($(val).attr("name") == "mobile") {
		  $(".phoneMsg").remove();
		  var phone = val.value;
		  var regPhone = /^1[3456789]\d{9}$/;
		  if (phone == "" || phone.trim() == "") {
			errMsg = "<span class = 'phoneMsg' style = 'color:red;font-size:14px;' > 手机号不能为空 </span>"
		  } else if (!regPhone.test(phone)) {
			errMsg = "<span class = 'phoneMsg' style = 'color:red;font-size:14px;' > 不是正确的手机号 </span>"
		  } else {
			errMsg = ""
		  }
		  $(this).parent().after(errMsg);
		} 
	  });
	});

	/*ajax*/ 

});