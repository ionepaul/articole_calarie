jQuery(document).ready(function ($) {
  "use strict";
    //menu onepage
    $(".each-section .next-section").on( "click", function(e) {
      var url = $(this).attr("href");
      var target = parseFloat($(url).offset().top); 
      $('html,body').animate({scrollTop:target}, 'slow');
      return false;
    });

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
     
    function kt_tab_fadeeffect(){
      // effect click
      $(document).on('click','.kt-tab-fadeeffect a[data-toggle="pill"]',function(){
        var item = '.product-item';
        var tab_id = $(this).attr('href');
        var tab_animated = $(this).data('animated');
        tab_animated = ( tab_animated == undefined ) ? 'fadeInUp' : tab_animated;

        if( $(tab_id).find('.owl-carousel').length > 0 ){
          item = '.owl-item.active';
        }
        $(tab_id).find(item).each(function(i){
          var t = $(this);
          var style = $(this).attr("style");
          style = ( style == undefined ) ? '' : style;
          var delay  = i * 200;
          t.attr("style", style +
                    ";-webkit-animation-delay:" + delay + "ms;"
                  + "-moz-animation-delay:" + delay + "ms;"
                  + "-o-animation-delay:" + delay + "ms;"
                  + "animation-delay:" + delay + "ms;"
          ).addClass(tab_animated+' animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function(){
              t.removeClass(tab_animated+' animated');
              t.attr("style", style);
          });
        })
      })
    }

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

    function sticky_menu(){
      if(!$('.header').hasClass('no-sticky')) {
        if(!$('.header').hasClass('no-prepend-box-sticky')){
          if (!$('.header .box-sticky').length) {
              $('.header').prepend('<div class="box-sticky"><div class="row"><div class="col-md-2 col-lg-2"><div class="logo-prepend"></div></div><div class="col-md-8 col-lg-8"><div class="main-menu-prepend"></div></div><div class="col-md-2 col-lg-2"><div class="top-links-prepend"><div class="wishlist-prepend prepend-icon"></div><div class="cart-prepend prepend-icon"></div></div></div></div></div>');
          }
        }
        $('.header').find('.logo').clone().appendTo('.header .logo-prepend');
        $('.header').find('.main-menu').clone().appendTo('.header .main-menu-prepend');
        $('.header').find('.wishlist-icon').clone().appendTo('.header .top-links-prepend .wishlist-prepend');
        $('.header').find('.minicart').clone().appendTo('.header .top-links-prepend .cart-prepend');  
      }
    }

     /*function sticky_menu_run(){
        if($(window).width() > 1024) {
            if ($(window).scrollTop() > 350) {
                $('.header .box-sticky').addClass('is-sticky');
                $('.header .this-sticky').addClass('box-sticky');
            } 
            else {
                $('.header .box-sticky').removeClass('is-sticky');
                $('.header .this-sticky').removeClass('box-sticky');
            }
        }
    }*/

    function kt_innit_carousel(){
        //owl has thumbs 
   
        // check if there's only 1 time in the thumbs
        var isMulti = ($('.owl-carousel .item').length > 1) ? true : false;
   
        $('.owl-carousel.has-thumbs').owlCarousel({
            loop: isMulti,
            items: 1,
            thumbs: true,
            thumbImage: true,
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
    $('.slider-range-price').each(function(){
      var min             = parseFloat($(this).data('min'));
      var max             = parseFloat($(this).data('max'));
      var unit            = $(this).data('unit');
      var value_min       = parseFloat($(this).data('value-min'));
      var value_max       = parseFloat($(this).data('value-max'));
      var label_reasult   = $(this).data('label-reasult');
      var t               = $(this);
      $('.price-filter' ).slider({
        range: true,
        min: min,
        max: max,
        values: [ value_min, value_max ],
        slide: function (event, ui) {
            var result = '<span class="from">' + ui.values[0].toFixed(2).replace(".", ",") + ' -' + ' </span><span class="to"> ' + ui.values[1].toFixed(2).replace(".", ",") + '</span>';
          t.closest('.price-filter').find('.amount-range-price').html('Price: ' + result + ' ' + unit);
        }
      });
    });

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

  /*special banner*/
    
    function special_banner() {
      $(".special-banner .banner-item").each(function(index, el) {
        var backgroundbanner = $(this).find('.banner-content').data('background');
         $(this).find('.banner-content').css({'background-image':'url( '+ backgroundbanner + ')'});
      });
      $(document).on('click','.banner-item .show-banner',function(){
        $(this).closest('.banner-item').find('.banner-content').addClass('show');
          return false;
        });
        $(document).on('click','.banner-item .close-banner',function(){
          $(this).closest('.banner-item').find('.banner-content').removeClass('show');
          return false;
        });
    } 
    function kt_scroll() {
      //if(parseFloat($(window).outerWidth()) > 0) {
      //  $('.header .scrollbar').mCustomScrollbar();
      //}
    }

    // function newletter_popup(){
    //   var window_size = parseFloat(jQuery('body').innerWidth());
    //   window_size += kt_get_scrollbar_width();
    //   if(window_size > 767){
    //     if($('body').hasClass('home')){
    //           $.magnificPopup.open({
    //              items: {
    //               src: '<div class="popup-newsletter "><div class="popup-content"><h4 class="title">SIGN UP NEWSLETTER</h4><h5 class="subtitle">Sign up our Newsletter & Get 25% Off at your first Purchase!</h5><div class="input-block inner-content"><div class="input-inner"><input type="text" class="input-info" placeholder="Enter your email" name="input-info"><a href="#" class="submit">Subscribe</a></div></div></div></div></div>',
    //               type: 'inline'
    //            }
    //            });
    //        }
    //   }
    // }

    function compare_popup(){
      var window_size = parseFloat(jQuery('body').innerWidth());
      window_size += kt_get_scrollbar_width();
      if(window_size > 600){
         $(document).on('click','.compare-button',function(){
              $.magnificPopup.open({
                 items: {
                  src: '<div class="popup-compare"><h4 class="supper-title">Compare products</h4><table class="compare-content"><tr><td class="product-title" data-title="Product image"><span>Product image</span></td><td class="product-img"><div class="product-item layout1"><div class="product-inner"><div class="thumb"><a href="#"><img src="images/product13.jpg" alt=""></a></div><div class="info"><a href="#" class="product-name">Classic T-Shirt in Blue</a><div class="price"><span class="ins">$75</span></div></div></div></div></td><td class="product-img"><div class="product-item layout1"><div class="product-inner"><div class="thumb"><a href="#"><img src="images/product80.jpg" alt=""></a></div><div class="info"><a href="#" class="product-name">Classic T-Shirt in Blue</a><div class="price"><span class="del">$180</span><span class="ins">$90</span></div></div></div></div></td><td class="product-img"><div class="product-item layout1"><div class="product-inner"><div class="thumb"><a href="#"><img src="images/product1.jpg" alt=""></a></div><div class="info"><a href="#" class="product-name">Classic T-Shirt in Blue</a><div class="price"><span class="ins">$75</span></div></div></div></div></td></tr><tr><td class="product-title" data-title="Descriptions"><span>Descriptions</span></td><td class="product-des"><p>Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Vestibulum tortor quam,feugiat vitae,</p></td><td class="product-des"><p>Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Vestibulum tortor quam,feugiat vitae,</p></td><td class="product-des"><p>Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Vestibulum tortor quam,feugiat vitae,</p></td></tr><tr><td class="product-title" data-title="Availability"><span>Availability</span></td><td class="availability"><span class="stock">In Stock</span></td><td class="availability"><span class="not-stock">Out of Stock</span></td><td class="availability"><span class="not-stock">Out of Stock</span></td></tr><tr><td class="product-title" data-title="Unit Price"><span>Unit Price</span></td><td class="unit-price"><span class="price">$150.00</span></td><td class="unit-price"><span class="price">$90.00</span></td><td class="unit-price"><span class="price">$150.00</span></td></tr></table></div>',
                  type: 'inline'
               }
               });
              return false;
           });
      }
    }

    $("#datecountdown").TimeCircles();
    var updateTime = function(){
      var date = parseInt($("#date").val());
      var time = parseInt($("#time").val());
      var datetime = date + ' ' + time + ':00';
      $("#datecountdown").data('date', datetime).TimeCircles().start();
    }

    $(".chosen-select").chosen({disable_search_threshold: 10});
    /*newletter_popup(); */
    kt_tab_fadeeffect();
    kt_resizeMegamenu();
    kt_verticalMegamenu();
    sticky_menu();
    kt_innit_carousel();
    special_banner();
    compare_popup();

    $(window).resize(function(){
      kt_resizeMegamenu();
      kt_verticalMegamenu();
      kt_innit_carousel();
      compare_popup();
    });
    $(window).load(function(){
      kt_scroll();
      kt_innit_carousel();
      //newletter_popup();
      compare_popup();
    });
});
