function TimeoutWarning($module) {
    this.$module = $module;
    if (!$module) {
        return;
    }
    this.$lastFocusedEl = null;
    this.$closeButton = $module.querySelector('.js-dialog-close');
    this.$cancelButton = $module.querySelector('.js-dialog-cancel');
    this.overLayClass = 'govuk-timeout-warning-overlay';
    this.timers = [];
    // UI countdown timer specific markup
    this.$countdown = $module.querySelector('.timer');
    this.$accessibleCountdown = $module.querySelector('.at-timer');
    // UI countdown specific settings
    this.minutesTimeOutModalVisible = $module.getAttribute('data-minutes-modal-visible') * 1;
    this.idleMinutesBeforeTimeOut = ($module.getAttribute('data-minutes-idle-timeout') * 1) - this.minutesTimeOutModalVisible;
    this.timeOutRedirectUrl = $module.getAttribute('data-url-redirect');
    this.timeUserLastInteractedWithPage = '';
    this.keepAliveUrl = $module.getAttribute('data-keep-alive-url');
    this.isOverrideTimeoutBackButton = $module.getAttribute('data-is-override-timeout-back-button') === "true";
    this.overrideTimeoutBackButtonUrl = $module.getAttribute('data-override-timeout-back-button-url');
}

// Initialise component
TimeoutWarning.prototype.init = function () {
    // Check for module
    if (!this.$module) {
        return;
    }

    // Check that dialog element has native or polyfill support
    if (!this.dialogSupported()) {
        return;
    }

    // Check for the existence of an element in the markup that indicates we should bypass this timer
    if (this.bypassTimeout()) {
        return;
    }

    // Start watching for idleness
    this.countIdleTime();

    this.$closeButton.addEventListener('click', this.closeDialog.bind(this));
    this.$module.addEventListener('keydown', this.escClose.bind(this));

    // Debugging tip: This event doesn't kick in in Chrome if you have Inspector panel open and have clicked on it
    // as it is now the active element. Click on the window to make it active before moving to another tab.
    window.addEventListener('focus', this.checkIfShouldHaveTimedOut.bind(this));
};

TimeoutWarning.prototype.getQuerystringParamValue = function (key) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] === key) {
            return pair[1];
        }
    }

    return "";
};

// Check if browser supports native dialog element or can use polyfill
TimeoutWarning.prototype.dialogSupported = function () {
    if (typeof HTMLDialogElement === 'function') {
        // Native dialog is supported by browser
        return true;
    } else {
        // Native dialog is not supported by browser so use polyfill
        try {
            window.dialogPolyfill.registerDialog(this.$module);
            return true;
        } catch (error) {
            // Doesn't support polyfill
            return false;
        }
    }
};

// Check for the existence of an element in the markup that indicates we should bypass this timer
TimeoutWarning.prototype.bypassTimeout = function () {
    if (document.getElementById("bypass-timeout") !== null) {
        // Element not present, good to continue with timeout initialization
        return true;
    } else {
        return false;
    }
};

// Count idle time (user not interacting with page)
// If user is idle for specified time period, open timeout warning as dialog
TimeoutWarning.prototype.countIdleTime = function () {
    // Allow override of the timeout value by providing an option to supply a querystring value instead.
    var overrideSessionTimeout = Number(this.getQuerystringParamValue("sessiontimeout"));
    if (overrideSessionTimeout > 0) {
        this.idleMinutesBeforeTimeOut = overrideSessionTimeout;
    }
    var milliSecondsBeforeTimeOut = this.idleMinutesBeforeTimeOut * 60000;

    // Start new idle time
    setTimeout(this.openDialog.bind(this), milliSecondsBeforeTimeOut);

    if (window.localStorage) {
        window.localStorage.setItem('timeUserLastInteractedWithPage', new Date());
    }
};

TimeoutWarning.prototype.openDialog = function () {
    if (!this.isDialogOpen()) {
        document.querySelector('body').classList.add(this.overLayClass);
        // Added this to support IE8 - cant use :not pseudo selector
        document.getElementById('js-timeout-warning').classList.add("dialog-open");
        this.saveLastFocusedEl();
        this.makePageContentInert();
        this.$module.showModal();

        this.startUiCountdown();
    }
};

// Starts a UI countdown timer. If timer is not cancelled before 0
// reached + 4 seconds grace period, user is redirected.
TimeoutWarning.prototype.startUiCountdown = function () {
    this.clearTimers(); // Clear any other modal timers that might have been running
    var $module = this;
    var $countdown = this.$countdown;
    var $accessibleCountdown = this.$accessibleCountdown;
    var minutes = this.minutesTimeOutModalVisible;
    var timerRunOnce = false;
    var iOS = /iPad|iPhone|iPod/.test(navigator.userAgent) && !window.MSStream;
    var timers = this.timers;

    var seconds = 60 * minutes;

    $countdown.innerHTML = minutes + ' minute' + (minutes > 1 ? 's' : '');

    (function runTimer() {
        var minutesLeft = parseInt(seconds / 60, 10);
        var secondsLeft = parseInt(seconds % 60, 10);
        var timerExpired = minutesLeft < 1 && secondsLeft < 1;

        var minutesText = minutesLeft > 0 ? '<span class="tabular-numbers">' + minutesLeft + '</span> minute' + (minutesLeft > 1 ? 's' : '') + '' : ' ';
        var secondsText = secondsLeft >= 1 ? ' <span class="tabular-numbers">' + secondsLeft + '</span> second' + (secondsLeft > 1 ? 's' : '') + '' : '';
        var atMinutesNumberAsText = '';
        var atSecondsNumberAsText = '';

        try {
            atMinutesNumberAsText = this.numberToWords(minutesLeft); // Attempt to convert numerics into text as iOS VoiceOver ccassionally stalled when encountering numbers
            atSecondsNumberAsText = this.numberToWords(secondsLeft);
        } catch (e) {
            atMinutesNumberAsText = minutesLeft;
            atSecondsNumberAsText = secondsLeft;
        }

        var atMinutesText = minutesLeft > 0 ? atMinutesNumberAsText + ' minute' + (minutesLeft > 1 ? 's' : '') + '' : '';
        var atSecondsText = secondsLeft >= 1 ? ' ' + atSecondsNumberAsText + ' second' + (secondsLeft > 1 ? 's' : '') + '' : '';

        // Below string will get read out by screen readers every time the timeout refreshes (every 15 secs. See below).
        // Please add additional information in the modal body content or in below extraText which will get announced to AT the first time the time out opens
        var text = 'Your application will be reset if you do not respond in ' + minutesText + secondsText + '.';
        var atText = 'Your application will be reset if you do not respond in ' + atMinutesText;
        if (atSecondsText) {
            if (minutesLeft > 0) {
                atText += ' and';
            }
            atText += atSecondsText + '.';
        } else {
            atText += '.';
        }
        var extraText = ' This keeps your information secure.';

        if (timerExpired) {
            $countdown.innerHTML = 'You are about to be redirected';
            $accessibleCountdown.innerHTML = 'You are about to be redirected';

            setTimeout($module.redirect.bind($module), 4000);
        } else {
            seconds--;

            $countdown.innerHTML = text + extraText;

            if (minutesLeft < 1 && secondsLeft < 20) {
                $accessibleCountdown.setAttribute('aria-live', 'assertive');
            }

            if (!timerRunOnce) {
                // Read out the extra content only once. Don't read out on iOS VoiceOver which stalls on the longer text

                if (iOS) {
                    $accessibleCountdown.innerHTML = atText;
                } else {
                    $accessibleCountdown.innerHTML = atText + extraText;
                }
                timerRunOnce = true;
            } else if (secondsLeft % 15 === 0) {
                // Update screen reader friendly content every 15 secs
                $accessibleCountdown.innerHTML = atText;
            }

            // JS doesn't allow resetting timers globally so timers need to be retained for resetting.
            timers.push(setTimeout(runTimer, 1000));
        }
    })();
};

TimeoutWarning.prototype.saveLastFocusedEl = function () {
    this.$lastFocusedEl = document.activeElement;
    if (!this.$lastFocusedEl || this.$lastFocusedEl === document.body) {
        this.$lastFocusedEl = null;
    } else if (document.querySelector) {
        this.$lastFocusedEl = document.querySelector(':focus');
    }
};

// Set focus back on last focused el when modal closed
TimeoutWarning.prototype.setFocusOnLastFocusedEl = function () {
    if (this.$lastFocusedEl) {
        window.setTimeout(function () {
            if (this.$lastFocusedEl) {
                this.$lastFocusedEl.focus();
            }
        }, 0);
    }
};

// Set page content to inert to indicate to screenreaders it's inactive
// NB: This will look for #content for toggling inert state
TimeoutWarning.prototype.makePageContentInert = function () {
    if (document.querySelector('#content')) {
        document.querySelector('#content').inert = true;
        document.querySelector('#content').setAttribute('aria-hidden', 'true');
    }
};

// Make page content active when modal is not open
// NB: This will look for #content for toggling inert state
TimeoutWarning.prototype.removeInertFromPageContent = function () {
    if (document.querySelector('#content')) {
        document.querySelector('#content').inert = false;
        document.querySelector('#content').setAttribute('aria-hidden', 'false');
    }
};

TimeoutWarning.prototype.isDialogOpen = function () {
    return this.$module['open'];
};

TimeoutWarning.prototype.closeDialog = function () {
    if (this.isDialogOpen()) {
        // Ping the server to touch the session - will perform 'keep alive'.
        this.performKeepAlive();

        document.querySelector('body').classList.remove(this.overLayClass);
        // Added this to support IE8 - cant use :not pseudo selector
        document.getElementById('js-timeout-warning').classList.remove("dialog-open");
        this.$module.close();
        this.setFocusOnLastFocusedEl();
        this.removeInertFromPageContent();

        this.clearTimers();

        // Re-start watching for idleness
        this.countIdleTime();
    }
};

// Clears modal timer
TimeoutWarning.prototype.clearTimers = function () {
    for (var i = 0; i < this.timers.length; i++) {
        clearTimeout(this.timers[i]);
    }
};

TimeoutWarning.prototype.disableBackButtonWhenOpen = function () {
    window.addEventListener('popstate', function () {
        if (this.isDialogOpen()) {
            this.closeDialog();
        } else {
            window.history.go(-1);
        }
    });
};

// Close modal when ESC pressed
TimeoutWarning.prototype.escClose = function (event) {
    // get the target element
    if (this.isDialogOpen() && event.keyCode === 27) {
        this.closeDialog();
    }
};

// Do a timestamp comparison with server when the page regains focus to check
// if the user should have been timed out already.
// This could happen but because the computer went to sleep, the browser crashed etc.
TimeoutWarning.prototype.checkIfShouldHaveTimedOut = function () {
    if (window.localStorage) {
        // If less time than data-minutes-idle-timeout left, call redirect
        var timeUserLastInteractedWithPage = new Date(window.localStorage.getItem('timeUserLastInteractedWithPage'));

        var seconds = Math.abs((timeUserLastInteractedWithPage - new Date()) / 1000);

        if (seconds > ((this.idleMinutesBeforeTimeOut * 60) + (this.minutesTimeOutModalVisible * 60))) {
            this.redirect.bind(this)();
        }
    }
};
TimeoutWarning.prototype.performKeepAlive = function () {
    var xhr = new XMLHttpRequest(),
        method = "GET",
        url = this.keepAliveUrl;

    xhr.open(method, url, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === XMLHttpRequest.DONE) {
            var status = xhr.status;
            if (status === 0 || (status >= 200 && status < 400)) {
                if (window.localStorage) {
                    window.localStorage.setItem('timeUserLastInteractedWithPage', new Date());
                }
            } else {
                console.error(xhr.responseText);
            }
        }
    };
    xhr.send();
};
TimeoutWarning.prototype.redirect = function () {
    if (this.isOverrideTimeoutBackButton === true) {
        // We wish to go to a separate location within the service if the user presses 'back' on the timeout page, so replace the current history state.
        history.pushState(null, null, this.overrideTimeoutBackButtonUrl);
    }
    window.location.href = this.timeOutRedirectUrl;
};
TimeoutWarning.prototype.numberToWords = function () {
    var string = n.toString();
    var units;
    var tens;
    var scales;
    var start;
    var end;
    var chunks;
    var chunksLen;
    var chunk;
    var ints;
    var i;
    var word;
    var words = 'and';

    if (parseInt(string) === 0) {
        return 'zero';
    }

    /* Array of units as words */
    units = ['', 'one', 'two', 'three', 'four', 'five', 'six', 'seven', 'eight', 'nine', 'ten', 'eleven', 'twelve', 'thirteen', 'fourteen', 'fifteen', 'sixteen', 'seventeen', 'eighteen', 'nineteen'];

    /* Array of tens as words */
    tens = ['', '', 'twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety'];

    /* Array of scales as words */
    scales = ['', 'thousand', 'million', 'billion', 'trillion', 'quadrillion', 'quintillion', 'sextillion', 'septillion', 'octillion', 'nonillion', 'decillion', 'undecillion', 'duodecillion', 'tredecillion', 'quatttuor-decillion', 'quindecillion', 'sexdecillion', 'septen-decillion', 'octodecillion', 'novemdecillion', 'vigintillion', 'centillion'];

    /* Split user argument into 3 digit chunks from right to left */
    start = string.length;
    chunks = [];
    while (start > 0) {
        end = start;
        chunks.push(string.slice((start = Math.max(0, start - 3)), end));
    }

    /* Check if function has enough scale words to be able to stringify the user argument */
    chunksLen = chunks.length;
    if (chunksLen > scales.length) {
        return '';
    }

    /* Stringify each integer in each chunk */
    words = [];
    for (i = 0; i < chunksLen; i++) {
        chunk = parseInt(chunks[i]);

        if (chunk) {
            /* Split chunk into array of individual integers */
            ints = chunks[i].split('').reverse().map(parseFloat);

            /* If tens integer is 1, i.e. 10, then add 10 to units integer */
            if (ints[1] === 1) {
                ints[0] += 10;
            }

            /* Add scale word if chunk is not zero and array item exists */
            if ((word = scales[i])) {
                words.push(word);
            }

            /* Add unit word if array item exists */
            if ((word = units[ints[0]])) {
                words.push(word);
            }

            /* Add tens word if array item exists */
            if ((word = tens[ints[1]])) {
                words.push(word);
            }

            /* Add hundreds word if array item exists */
            if ((word = units[ints[2]])) {
                words.push(word + ' hundred');
            }
        }
    }
    return words.reverse().join(' ');
};

var $timeoutWarning = document.querySelector('[data-module="govuk-timeout-warning"]');
new TimeoutWarning($timeoutWarning).init();
/* Cookie banner */
(function () {
    var cookieBannerModule = (function () {
        'use strict';
        
        /* Reference elements */
        var $cookieMessage = document.getElementById("cookie-message");
        var $cookieBannerHideButton = document.getElementById("jsHideLink"); 
       
        function _hideBanner() {
            if ($cookieBannerHideButton !== null) {
                $cookieBannerHideButton.addEventListener("click", function () {
                    $cookieMessage.classList.add('u-hide')
                }, false);
            }
        }
        return {
            hideBanner: function () {
                _hideBanner(); 
            }
        };
    })();
    cookieBannerModule.hideBanner();
})();
