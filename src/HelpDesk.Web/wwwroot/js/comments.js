var commentArea = document.querySelector('#comment');
var addCommentField = document.querySelector('.added_comment')
var addCommentButtons = document.querySelectorAll('.comment_button');
var cancelAddComment = document.querySelector('.revert_comment_button');

commentArea.addEventListener('invalid', function (e) {
    e.target.setCustomValidity('');
    if (!e.target.validity.valid) {
        e.target.setCustomValidity('Пожалуйста, введите комментарий.');
    }
});

commentArea.addEventListener('input', function (e) {
    e.target.setCustomValidity('');
});

commentArea.addEventListener('keydown', autosize);

document.addEventListener("DOMContentLoaded",setListenerToButton);

function autosize () {
    var el = this;
    setTimeout(function () {
        el.style.cssText = 'height:auto;';
        el.style.cssText = 'height:' + el.scrollHeight + 'px';
    }, 0);
}

function setListenerToButton(){
    addCommentButtons.forEach(
        function (element){                             
        element.addEventListener("click",clickAddComment)}
    );
}

function clickAddComment(){
    addCommentField.removeAttribute("style")
    addCommentButtons.forEach(function(element){
        element.setAttribute("style","display:none;")
    });
    cancelAddComment.addEventListener("click",deleteComment);
}

function deleteComment(){
    addCommentField.setAttribute("style","display:none;");
    addCommentButtons.forEach(function(element){
        element.removeAttribute("style")
    });
}