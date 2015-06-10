$(document).ready(function(){
    $('.tabs li a').click(function () {
      $('.tabs li').removeClass('active');
      $(this).parent().addClass('active');

      $('.nav').hide();
      var index = $('.tabs li a').index(this);
      $('.nav').eq(index).show();
      return false;
    });

    $('.nav li').has('ul').hover(function(){
        $(this).addClass('current').children('ul').fadeIn();
    }, function() {
        $(this).removeClass('current').children('ul').hide();
    });
});
