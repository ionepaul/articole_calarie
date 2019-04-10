jQuery(document).ready(function ($) {
    "use strict";

    $(window).scroll(function(){
        if ($(this).scrollTop() > 300) {
            $('.back-to-top').fadeIn();
            $('.back-to-top').addClass('show');
        } else {
            $('.back-to-top').fadeOut();
            $('.back-to-top').removeClass('show');
        }
  });

    $(document).on('click','.back-to-top',function(){
        $('html, body').animate({scrollTop : 0},800);
        return false;
    });
     
    function kt_get_scrollbar_width() {
      var $inner = jQuery('<div style="width: 100%; height:200px;">test</div>'),
          $outer = jQuery('<div style="width:200px;height:150px; position: absolute; top: 0; left: 0; visibility: hidden; overflow:hidden;"></div>').append($inner),
          inner = $inner[0],
          outer = $outer[0];
      jQuery('body').append(outer);
      var width1 = parseFloat(inner.offsetWidth);
      $outer.css('overflow', 'scroll');
      var width2 = parseFloat(outer.clientWidth);
      $outer.remove();
      return (width1 - width2);
    }

    function kt_resizeMegamenu(){
      var window_size = parseFloat(jQuery('body').innerWidth());
      window_size += kt_get_scrollbar_width();
      if( window_size > 1024 ){
        if( $('.container-wapper .main-menu').length > 0){
          var container = $('.main-menu-wapper');
          if( container!= 'undefined'){
            var container_width = 0;
            container_width = parseFloat(container.innerWidth());
            var container_offset = 0;
            container_offset = container.offset();
            setTimeout(function(){
              $('.main-menu .menu-item-has-children').each(function(index,element){
                $(element).children('.mega-menu').css({'width':container_width+'px'});
                var sub_menu_width = parseFloat($(element).children('.mega-menu').outerWidth());
                var item_width = parseFloat($(element).outerWidth());
                $(element).children('.mega-menu').css({'left':'-'+(sub_menu_width/2 - item_width/2)+'px'});
                var container_left = container_offset.left;
                var container_right = (container_left + container_width);
                var item_left = $(element).offset().left;
                var overflow_left = (sub_menu_width/2 > (item_left - container_left));
                var overflow_right = ((sub_menu_width/2 + item_left) > container_right);
                if( overflow_left ){
                  var left = (item_left - container_left);
                  $(element).children('.mega-menu').css({'left':-left+'px'});
                }
                if( overflow_right && !overflow_left ){
                  var left = (item_left - container_left);
                  left = left - ( container_width - sub_menu_width );
                  $(element).children('.mega-menu').css({'left':-left+'px'});
                }
              })
            },100);
          }
        }
      }
    }

    function kt_innit_carousel(){
        //owl has thumbs    
        // check if there's only 1 time in the thumbs
        var isMulti = ($('.owl-carousel .item').length > 1) ? true : false;
   
        $('.owl-carousel.has-thumbs').owlCarousel({
            loop: isMulti,
            items: 1,
            thumbs: true,
            thumbImage: true,
            autoHeight: true,
            thumbContainerClass: 'owl-thumbs',
            thumbItemClass: 'owl-thumb-item'
        });
        // owl config
        $(".owl-carousel").each(function(index, el) {
            var config = $(this).data();
            config.navText = ['<span class="flaticon-arrows-left"></span>','<span class="flaticon-arrows-right"></span>'];
            var animateOut = $(this).data('animateout');
            var animateIn  = $(this).data('animatein');
            var slidespeed = parseFloat($(this).data('slidespeed'));
           
            if(typeof animateOut != 'undefined' ){
                config.animateOut = animateOut;
            }
            if(typeof animateIn != 'undefined' ){
                config.animateIn = animateIn;
            }
            if(typeof (slidespeed) != 'undefined' ){
                config.smartSpeed = slidespeed;
            }

            if( $('body').hasClass('rtl')){
                config.rtl = true;
            }

            var owl = $(this);
            owl.on('initialized.owl.carousel',function(event){
                var total_active = parseInt(owl.find('.owl-item.active').length);
                var i            = 0;
                owl.find('.owl-item').removeClass('item-first item-last');
                setTimeout(function(){
                    owl.find('.owl-item.active').each(function () {
                        i++;
                        if (i == 1) {
                            $(this).addClass('item-first');
                        }
                        if (i == total_active) {
                            $(this).addClass('item-last');
                        }
                    });

                }, 100);
            })
            owl.on('refreshed.owl.carousel',function(event){
                var total_active = parseInt(owl.find('.owl-item.active').length);
                var i            = 0;
                owl.find('.owl-item').removeClass('item-first item-last');
                setTimeout(function(){
                    owl.find('.owl-item.active').each(function () {
                        i++;
                        if (i == 1) {
                            $(this).addClass('item-first');
                        }
                        if (i == total_active) {
                            $(this).addClass('item-last');
                        }
                    });

                }, 100);
            })
            owl.on('change.owl.carousel',function(event){
                var total_active = parseInt(owl.find('.owl-item.active').length);
                var i            = 0;
                owl.find('.owl-item').removeClass('item-first item-last');
                setTimeout(function(){
                    owl.find('.owl-item.active').each(function () {
                        i++;
                        if (i == 1) {
                            $(this).addClass('item-first');
                        }
                        if (i == total_active) {
                            $(this).addClass('item-last');
                        }
                    });

                }, 100);
                owl.addClass('owl-changed');
                setTimeout(function () {
                    owl.removeClass('owl-changed');
                },config.smartSpeed)
            })
            owl.on('drag.owl.carousel',function(event){
                owl.addClass('owl-changed');
                setTimeout(function () {
                    owl.removeClass('owl-changed');
                },config.smartSpeed)
            })
            if($(window).width() < 992)  {
                var itembackground = $(".item-background");
                    itembackground.each(function(index){
                    if ($('.item-background').attr("data-background")){
                      $(this).css("background-image", "url(" + $(this).data("background") + ")");
                      var height = parseInt($(this).closest('.owl-carousel').data("height"));
                      $(this).css("height", height + 'px');
                      $('.slide-img').css("display",'none');
                    }
                });
            } 
            owl.owlCarousel(config);

        });
    }

    function kt_verticalMegamenu(){
        var window_size = parseFloat(jQuery('body').innerWidth());
        window_size += kt_get_scrollbar_width();
        if( window_size > 991 ){
            if( parseFloat($('.container-vertical-wapper .vertical-menu').length) >0){
                var container = $('.container-vertical-wapper');
                if( container!= 'undefined'){
                    var container_width = 0;
                    container_width = parseFloat(container.innerWidth());
                    var container_offset = 0;
                    container_offset = container.offset();
                    var content_width = 0;
                    content_width = parseFloat($('.vertical-wapper ').outerWidth());
                    setTimeout(function(){
                        $('.vertical-menu .menu-item-has-children').each(function(index,element){
                             $(element).children('.mega-menu').css({'width':container_width - content_width + 30 + 'px'});
                             console.log(container_width - content_width );
                        })
                    },100);
                }
            }
        }
    }

    // Price filter
    document.querySelectorAll('.nouirange').forEach(function (el) {
        let htmlinsert = document.createElement('div');
        let realmininput = el.querySelector('.min');
        let realmaxinput = el.querySelector('.max');
        realmininput.style.display = "none ";
        realmaxinput.style.display = "none ";

        let min = Number(realmininput.getAttribute('min').replace(",", "."));
        let max = Number(realmaxinput.getAttribute('max').replace(",", "."));

        el.appendChild(htmlinsert);

        var startValue = Number(realmininput.getAttribute('value').replace(",","."));
        var endValue = Number(realmaxinput.getAttribute('value').replace(",", "."));

        noUiSlider.create(htmlinsert, {
            start: [startValue, endValue],
            connect: true,
            range: {
                'min': min,
                'max': max
            },
            step: 1
        });

        htmlinsert.noUiSlider.on('change', function (values) {
            let rangevals = values;
            realmininput.value = String(values[0]);
            realmaxinput.value = String(values[1]);
            $("#priceFilterMin").text(values[0].replace(".", ","));
            $("#priceFilterMax").text(values[1].replace(".", ","));
        });

        htmlinsert.noUiSlider.on('update', function (values, handle) {
            let rangevals = values;
            $("#priceFilterMin").text(values[0].replace(".", ","));
            $("#priceFilterMax").text(values[1].replace(".", ","));
        });
    })


    //Woocommerce plus and minius
    $(document).on('click', '.quantity .plus, .quantity .minus', function (e) {
        // Get values
        var $qty = $(this).closest('.quantity').find('.qty'),
            currentVal = parseFloat($qty.val()),
            max = parseFloat($qty.attr('max')),
            min = parseFloat($qty.attr('min')),
            step = $qty.attr('step');
        // Format values
        if (!currentVal || currentVal === '' || currentVal === 'NaN') currentVal = 0;
        if (max === '' || max === 'NaN') max = '';
        if (min === '' || min === 'NaN') min = 0;
        if (step === 'any' || step === '' || step === undefined || parseFloat(step) === 'NaN') step = 1;
        // Change the value
        if ($(this).is('.plus')) {
            if (max && ( max == currentVal || currentVal > max )) {
                $qty.val(max);
            } else {
                $qty.val(currentVal + parseFloat(step));
            }
        } else {
            if (min && ( min == currentVal || currentVal < min )) {
                $qty.val(min);
            } else if (currentVal > 0) {
                $qty.val(currentVal - parseFloat(step));
            }
        }
        // Trigger change event
        $qty.trigger('change');
        e.preventDefault();
    });

    $(document).on('click','.show-content',function(){
      $(this).closest('.parent-content').toggleClass('active');
      $(this).closest('.parent-content').find('.hidden-content').toggleClass('show');
      return false;
    });

    $(document).on('click','.header .show-menu',function(){
      $(this).closest('.header-nav').find('.box-menu').addClass('show');
      return false;
    });
    $(document).on('click','.header .close-menu',function(){
      $(this).closest('.header-nav').find('.box-menu').removeClass('show');
      return false;
    });

    $(document).on('click','.header.layout2 .menu-item-has-children > a',function(){
      $(this).closest('.menu-item-has-children').toggleClass('show-submenu');
      return false;
    });

    $(document).on('click','.vertical-menu .toggle-submenu',function(){
      $(this).closest('.menu-item-has-children').toggleClass('show-submenu');
      return false;
    });

    kt_resizeMegamenu();
    kt_verticalMegamenu();

    if ($('.owl-carousel').length > 0) {
        kt_innit_carousel();
    }
   
    $(window).resize(function(){
      kt_resizeMegamenu();
      kt_verticalMegamenu();
      if ($('.owl-carousel').length > 0) {
          kt_innit_carousel();
      }    
    });

    $(window).load(function() {
        if ($('.owl-carousel').length > 0) {
            kt_innit_carousel();
        } 
    });
});
