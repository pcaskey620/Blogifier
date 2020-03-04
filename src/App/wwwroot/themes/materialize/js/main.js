(function ($) {
    $(function () {

        $('.sidenav').sidenav();
        $('.parallax').parallax();

        hookupWayPoints();

        addScrollableLinks();
        if (window.location.href == "https://www.parksandwils.com/posts/winter-vanlife-essentials") {
            hookupStickyNav();
        }         
    }); // end of document ready
})(jQuery); // end of jQuery name space

function toggleMobileSearchField() {
    //$('#mobile-nav-search-section').slideToggle('fast');
    //if ($('#mobile-nav-search-section').is(":visible")) {
    //    $('#search').focus();
    //}
    $('#search').focus();
};

function toggleLargeSearchField() {
    $('#search-lg-container').slideToggle('fast', 'swing', function () {
        if ($('#search-lg-container').is(":visible")) {
            $('#search-lg').focus();
            $('#search-icon-lg').html('close');
        }
        if ($('#search-lg-container').is(":hidden")) {
            $('#search-icon-lg').html('search');
        }
    });
};

function searchSiteClick() {
    var term = $('#search').val();
    window.location.href = "https://www.parksandwils.com/blog?term=" + term;
};

function searchSiteLargeClick() {
    var term = $('#search-lg').val();
    window.location.href = "https://www.parksandwils.com/blog?term=" + term;
};

function getParamByNameLower(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS, "i");
    var results = regex.exec(window.location.search);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " ")).toLowerCase();
};

$(document).on('keypress', function (e) {
    var searchIsFocus = $('#search').is(':focus');
    var searchLargeIsFocus = $('#search-lg').is(':focus');
    if (e.which == 13 && (searchIsFocus || searchLargeIsFocus)) {
        if (searchIsFocus) {
            searchSiteClick();
        }
        if (searchLargeIsFocus) {
            searchSiteLargeClick();
        }
    }
});

function addScrollableLinks() {
    $('.scroll-link').click(function (e) {
        var targetHref = $(this).attr('href');
        $('html,body').animate({ scrollTop: $(targetHref).offset().top - 222 }, 1000);
        e.preventDefault();
    });
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('#mainNav').addClass('colored');
        } else {
            $('#mainNav').removeClass('colored');
        }
    });
};

function hookupWayPoints() {
    $('.wp1').waypoint(function () {
        $('.wp1').addClass('animated fadeInUp');
    }, {
            offset: '75%'
        });
    $('.wp2').waypoint(function () {
        $('.wp2').addClass('animated fadeInLeft');
    }, {
            offset: '75%'
        });
    $('.wp3').waypoint(function () {
        $('.wp3').addClass('animated fadeInRight');
    }, {
            offset: '75%'
        });
    $('.wp4').waypoint(function () {
        $('.wp4').addClass('animated fadeInLeft');
    }, {
            offset: '75%'
        });
    $('.wp5').waypoint(function () {
        $('.wp5').addClass('animated fadeInRight');
    }, {
            offset: '75%'
        });
    $('.wp6').waypoint(function () {
        $('.wp6').addClass('animated fadeInUp');
    }, {
            offset: '75%'
        });
    $('.wp7').waypoint(function () {
        $('.wp7').addClass('animated fadeInUp');
    }, {
            offset: '75%'
        });
    $('.wp8').waypoint(function () {
        $('.wp8').addClass('animated fadeIn');
    }, {
            offset: '75%'
        });
    $('.wp9').waypoint(function () {
        $('.wp9').addClass('animated fadeIn');
    }, {
            offset: '75%'
        });
    $('.wp10').waypoint(function () {
        $('.wp10').addClass('animated fadeInUp');
    }, {
            offset: '75%'
    });
    $('.wp11').waypoint(function () {
        $('.wp11').addClass('animated fadeInUp');
    }, {
            offset: '75%'
    });
    $('.wp12').waypoint(function () {
        $('.wp12').addClass('animated zoomInDown');
    }, {
            offset: '75%'
        });

};

function hookupStickyNav() {
    if (!IntersectionObserver) {
        return;
    }
    observeStickyHeaderChanges(document.querySelector('#scroll-container'));

    
    var intersectionobserveroptions = {
        root: null,//document.querySelector('#myRoot'),
        rootMargin: '-210px 0px 0px 0px'
        //threshold: [0.0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0]
        //threshold: [0.1]
    }


    const obs = new IntersectionObserver(onIntersecting, intersectionobserveroptions);

    document.querySelectorAll('.winter-section').forEach(section => {
        obs.observe(section);
    });

    //var targetVanStuff = document.querySelector('#van-stuff');
    //var targetGear = document.querySelector('#ski-gear');
    //var targetClothing = document.querySelector('#clothing');
    //var targetElectronics = document.querySelector('#electronics');

    
    //obs = new IntersectionObserver(onChangeCallback, intersectionObserverOptions);
    //obs = new IntersectionObserver(onIntersection, intersectionObserverOptions);

    //obs.observe(targetVanStuff);
    //obs.observe(targetGear);
    //obs.observe(targetClothing);
    //obs.observe(targetElectronics);
};



function onChangeCallback(entries, observer) {
    if (entries.length == 1) {
        entries.forEach(entry => {
            if (entry.isIntersecting && entry.intersectionRatio > 0.15) {
                var current_id = change.target.id;
                console.log(current_id);
                console.log(change.target.intersectionRatio);
                switch (current_id) {
                    case 'van-stuff':
                        $('#van-stuff-link').addClass('white-text');
                        $('#van-stuff-link').addClass('active');
                        $('#ski-gear-link').removeClass('white-text');
                        $('#ski-gear-link').removeClass('active');
                        $('#clothing-link').removeClass('white-text');
                        $('#clothing-link').removeClass('active');
                        $('#electronics-link').removeClass('white-text');
                        $('#electronics-link').removeClass('active');
                        break;
                    case 'ski-gear':
                        $('#ski-gear-link').addClass('white-text');
                        $('#ski-gear-link').addClass('active');
                        $('#van-stuff-link').removeClass('white-text');
                        $('#van-stuff-link').removeClass('active');
                        $('#clothing-link').removeClass('white-text');
                        $('#clothing-link').removeClass('active');
                        $('#electronics-link').removeClass('white-text');
                        $('#electronics-link').removeClass('active');
                        break;
                    case 'clothing':
                        $('#clothing-link').addClass('white-text');
                        $('#clothing-link').addClass('active');
                        $('#ski-gear-link').removeClass('white-text');
                        $('#ski-gear-link').removeClass('active');
                        $('#van-stuff-link').removeClass('white-text');
                        $('#van-stuff-link').removeClass('active');
                        $('#electronics-link').removeClass('white-text');
                        $('#electronics-link').removeClass('active');
                        break;
                    case 'electronics':
                        $('#electronics-link').addClass('white-text');
                        $('#electronics-link').addClass('active');
                        $('#ski-gear-link').removeClass('white-text');
                        $('#ski-gear-link').removeClass('active');
                        $('#clothing-link').removeClass('white-text');
                        $('#clothing-link').removeClass('active');
                        $('#van-stuff-link').removeClass('white-text');
                        $('#van-stuff-link').removeClass('active');
                        break;
                    default:
                        $('#van-stuff-link').addClass('white-text');
                        $('#van-stuff-link').addClass('active');
                        $('#ski-gear-link').removeClass('white-text');
                        $('#ski-gear-link').removeClass('active');
                        $('#clothing-link').removeClass('white-text');
                        $('#clothing-link').removeClass('active');
                        $('#electronics-link').removeClass('white-text');
                        $('#electronics-link').removeClass('active');
                        break;
                }
            }
        });
    } else {
        var firstRatio = changes[0].intersectionRatio;
        var secondRatio = changes[1].intersectionRatio;
        var useThisOne;
        if (firstRatio > secondRatio) {
            useThisOne = changes[0];
        } else if (secondRatio > firstRatio) {
            useThisOne = changes[1];
        }
        var useID = useThisOne.target.id;
        console.log(useID);
        console.log(useThisOne.target.intersectionRatio);
        switch (useID) {
            case 'van-stuff':
                $('#van-stuff-link').addClass('white-text');
                $('#van-stuff-link').addClass('active');
                $('#ski-gear-link').removeClass('white-text');
                $('#ski-gear-link').removeClass('active');
                $('#clothing-link').removeClass('white-text');
                $('#clothing-link').removeClass('active');
                $('#electronics-link').removeClass('white-text');
                $('#electronics-link').removeClass('active');
                break;
            case 'ski-gear':
                $('#ski-gear-link').addClass('white-text');
                $('#ski-gear-link').addClass('active');
                $('#van-stuff-link').removeClass('white-text');
                $('#van-stuff-link').removeClass('active');
                $('#clothing-link').removeClass('white-text');
                $('#clothing-link').removeClass('active');
                $('#electronics-link').removeClass('white-text');
                $('#electronics-link').removeClass('active');
                break;
            case 'clothing':
                $('#clothing-link').addClass('white-text');
                $('#clothing-link').addClass('active');
                $('#ski-gear-link').removeClass('white-text');
                $('#ski-gear-link').removeClass('active');
                $('#van-stuff-link').removeClass('white-text');
                $('#van-stuff-link').removeClass('active');
                $('#electronics-link').removeClass('white-text');
                $('#electronics-link').removeClass('active');
                break;
            case 'electronics':
                $('#electronics-link').addClass('white-text');
                $('#electronics-link').addClass('active');
                $('#ski-gear-link').removeClass('white-text');
                $('#ski-gear-link').removeClass('active');
                $('#clothing-link').removeClass('white-text');
                $('#clothing-link').removeClass('active');
                $('#van-stuff-link').removeClass('white-text');
                $('#van-stuff-link').removeClass('active');
                break;
            default:
                $('#van-stuff-link').addClass('white-text');
                $('#van-stuff-link').addClass('active');
                $('#ski-gear-link').removeClass('white-text');
                $('#ski-gear-link').removeClass('active');
                $('#clothing-link').removeClass('white-text');
                $('#clothing-link').removeClass('active');
                $('#electronics-link').removeClass('white-text');
                $('#electronics-link').removeClass('active');
                break;
        }
    }
    
};

function onIntersecting(entries) {
    entries.forEach(entry => {
        const id = entry.target.getAttribute('id');
        var selector = 'a[href="#' + id + '"]';
        if (entry.intersectionRatio > 0) {
            $(selector).addClass('active');
            $(selector).addClass('white-text');
        } else {
            $(selector).removeClass('active');
            $(selector).removeClass('white-text');
        }
    });
};

function onIntersection(entries) {
    //entries.forEach(entry => {
    //    console.clear();
    //    console.log(entry.intersectionRatio)
    //    //target.classList.toggle('visible', entry.intersectionRatio > 0);

    //    // Are we in viewport?
    //    if (entry.intersectionRatio > 0) {
    //        // Stop watching 
    //        // observer.unobserve(entry.target);
    //    }
    //});

    //var MaxFoundIndex = 0;
    //var MaxFoundRatio = 0;

    for (let i = 0; i < entries.length; i++) {
        //if (entries[i].intersectionRatio > MaxFoundRatio) {
//            MaxFoundRatio = entries[i].intersectionRatio;
//            MaxFoundIndex = i;
        var FoundID = entries[i].target.id;
        console.log(FoundID);
        console.log(entries[i].intersectionRatio);
        if (entries[i].intersectionRatio >= 0.1) {
            
            switch (FoundID) {
                case 'van-stuff':
                    $('#van-stuff-link').addClass('white-text');
                    $('#van-stuff-link').addClass('active');
                    $('#ski-gear-link').removeClass('white-text');
                    $('#ski-gear-link').removeClass('active');
                    $('#clothing-link').removeClass('white-text');
                    $('#clothing-link').removeClass('active');
                    $('#electronics-link').removeClass('white-text');
                    $('#electronics-link').removeClass('active');
                    break;
                case 'ski-gear':
                    $('#ski-gear-link').addClass('white-text');
                    $('#ski-gear-link').addClass('active');
                    $('#van-stuff-link').removeClass('white-text');
                    $('#van-stuff-link').removeClass('active');
                    $('#clothing-link').removeClass('white-text');
                    $('#clothing-link').removeClass('active');
                    $('#electronics-link').removeClass('white-text');
                    $('#electronics-link').removeClass('active');
                    break;
                case 'clothing':
                    $('#clothing-link').addClass('white-text');
                    $('#clothing-link').addClass('active');
                    $('#ski-gear-link').removeClass('white-text');
                    $('#ski-gear-link').removeClass('active');
                    $('#van-stuff-link').removeClass('white-text');
                    $('#van-stuff-link').removeClass('active');
                    $('#electronics-link').removeClass('white-text');
                    $('#electronics-link').removeClass('active');
                    break;
                case 'electronics':
                    $('#electronics-link').addClass('white-text');
                    $('#electronics-link').addClass('active');
                    $('#ski-gear-link').removeClass('white-text');
                    $('#ski-gear-link').removeClass('active');
                    $('#clothing-link').removeClass('white-text');
                    $('#clothing-link').removeClass('active');
                    $('#van-stuff-link').removeClass('white-text');
                    $('#van-stuff-link').removeClass('active');
                    break;
                default:
                    $('#van-stuff-link').addClass('white-text');
                    $('#van-stuff-link').addClass('active');
                    $('#ski-gear-link').removeClass('white-text');
                    $('#ski-gear-link').removeClass('active');
                    $('#clothing-link').removeClass('white-text');
                    $('#clothing-link').removeClass('active');
                    $('#electronics-link').removeClass('white-text');
                    $('#electronics-link').removeClass('active');
                    break;
            }
        } 


        //}
    }

    

}

function inspect() {

};

function observeStickyHeaderChanges(container) {
    observeHeaders(container);
    observeFooters(container);
}
function observeHeaders(container) {
    const observer = new IntersectionObserver((records, observer) => {
        for (const record of records) {
            const targetInfo = record.boundingClientRect;
            const stickyTarget = record.target.parentElement.querySelector('.sticky');
            const rootBoundsInfo = record.rootBounds;

            // Started sticking.
            if (targetInfo.bottom < rootBoundsInfo.top) {
                fireEvent(true, stickyTarget);
            }

            // Stopped sticking.
            if (targetInfo.bottom >= rootBoundsInfo.top &&
                targetInfo.bottom < rootBoundsInfo.bottom) {
                fireEvent(false, stickyTarget);
            }
        }
    }, { threshold: [0], root: container });

    // Add the top sentinels to each section and attach an observer.
    const sentinels = addSentinels(container, 'sticky_sentinel--top');
    sentinels.forEach(el => observer.observe(el));
    }

function observeFooters(container) {
    const observer = new IntersectionObserver((records, observer) => {
        for (const record of records) {
            const targetInfo = record.boundingClientRect;
            const stickyTarget = record.target.parentElement.querySelector('.sticky');
            const rootBoundsInfo = record.rootBounds;
            const ratio = record.intersectionRatio;

            // Started sticking.
            if (targetInfo.bottom > rootBoundsInfo.top && ratio === 1) {
                fireEvent(true, stickyTarget);
            }

            // Stopped sticking.
            if (targetInfo.top < rootBoundsInfo.top &&
                targetInfo.bottom < rootBoundsInfo.bottom) {
                fireEvent(false, stickyTarget);
            }
        }
    }, { threshold: [1], root: container });

    // Add the bottom sentinels to each section and attach an observer.
    const sentinels = addSentinels(container, 'sticky_sentinel--bottom');
    sentinels.forEach(el => observer.observe(el));
}


/**
* @param {!Element} container
* @param {string} className
*/
function addSentinels(container, className) {
    return Array.from(container.querySelectorAll('.sticky')).map(el => {
        const sentinel = document.createElement('div');
        sentinel.classList.add('sticky_sentinel', className);
        return el.parentElement.appendChild(sentinel);
    });
};

/**
    * Dispatches the `sticky-event` custom event on the target element.
    * @param {boolean} stuck True if `target` is sticky.
    * @param {!Element} target Element to fire the event on.
    */
function fireEvent(stuck, target) {
    const e = new CustomEvent('sticky-change', { detail: { stuck, target } });
    document.dispatchEvent(e);
    };

document.addEventListener('sticky-change', e => {
    const header = e.detail.target;  // header became sticky or stopped sticking.
    const sticking = e.detail.stuck; // true when header is sticky.

});