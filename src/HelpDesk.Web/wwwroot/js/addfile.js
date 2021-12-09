var addfile = document.querySelector('.myfile');
var addfileField = document.querySelector('.added_file');
var addfileButtons = document.querySelectorAll('.addfile_button');
var cancelAddFile = document.querySelector('.revert_addfile_button');

addfile.addEventListener('change', function (input) {
let lableVal = document.querySelector('.file_name_field').innerText;   
let fileName = input.target.files[0].name
document.querySelector('.file_name_field').innerText = fileName 
   console.log(fileName)   
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