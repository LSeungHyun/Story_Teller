mergeInto(LibraryManager.library, {
    CopyToClipboard: function(strPtr) {
        // strPtr을 UTF8 문자열로 변환
        var text = UTF8ToString(strPtr);

        // 브라우저 Clipboard API 호출
        // (HTTPS 환경에서, 사용자 액션(클릭 등) 이벤트 내에서만 동작)
        navigator.clipboard.writeText(text).then(function() {
            console.log("Text copied to clipboard: " + text);
        }).catch(function(err) {
            console.error("Failed to copy text: ", err);
        });
    }
});
