var addfile = document.querySelector('.myfile');
var addfileField = document.querySelector('.added_file');
var addfileButtons = document.querySelectorAll('.addfile_button');
var cancelAddFile = document.querySelector('.revert_addfile_button');
var endAddFileButton = document.querySelector('#addmyfile');
var messageFileSize = document.querySelector('.file_size_message');


addfile.addEventListener('change', function (input) {
let fileName = input.target.files[0].name
document.querySelector('.file_name_field').innerText = fileName 

if (!window.FileReader) { // This is VERY unlikely, browser support is near-universal
    console.log("The file API isn't supported on this browser yet.");
    return;
}
if (!addfile.files) { 
    console.error("This browser doesn't seem to support the `files` property of file inputs.");
}
else {
    var file = addfile.files[0];
    if(file.size > 10485760){
        var sizeFile = Math.round(parseInt(file.size)/1024/1024);
        messageFileSize.innerText = "Ваш файл превышает допустимый размер в 10Мб: "+ sizeFile+"Мб"; 
        messageFileSize.setAttribute('style','color:red;');
        endAddFileButton.setAttribute('type','reset'); 
    }
    else{
        messageFileSize.innerText = "Размер файла не должен превышать 10Mb";
        messageFileSize.removeAttribute('style');
        endAddFileButton.setAttribute('type','submit'); 

    }
}
});

function setInput(){    
    addfileButtons.forEach(
            function (element){                             
            element.addEventListener("click",clickAddFile)}
        );        
}

function clickAddFile(){
    addfileField.removeAttribute("style")
    addfileButtons.forEach(function(element){
        element.setAttribute("style","display:none;")
    });
    cancelAddFile.addEventListener("click",deleteFileField);
}

function deleteFileField(){
    addfileField.setAttribute("style","display:none;");
    addfileButtons.forEach(function(element){
        element.removeAttribute("style")
    });
}

document.addEventListener("DOMContentLoaded",setInput);