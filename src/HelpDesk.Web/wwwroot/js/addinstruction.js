var addfileButton = document.querySelector("#addmyfile");
var resultElement = document.querySelector(".result");
const file = document.getElementById("FileUpload_FormFile");
const form = document.querySelector(".added_file_form");
var uploadFileElement = document.querySelector(".upload_instruction_info");
var refuseFileButton = document.querySelector(".revert_instruction_button");


function uploadFile(input){
  let fileName = input.target.files[0].name
  document.querySelector('.file_name').innerText = fileName
  form.setAttribute("style","display:none;")
  uploadFileElement.setAttribute("style","display:flex;")
}

function skipFile(){
  uploadFileElement.removeAttribute("style");
  form.removeAttribute("style");
  resultElement.removeAttribute("style");
}

function setInput(){  
  addfileButton.addEventListener("click",sendFile);
  file.addEventListener('change', uploadFile);
  refuseFileButton.addEventListener('click', skipFile);
}   

  async function arro(data){
    var t = await data.text();    
    JSON.parse(t,(key, value) => {
      if(value!=null&&key!=""){
        showData(value);
      }
    });
  }

  function showData(value){  
    resultElement.innerHTML='';
    let addblock = document.createElement('div')
      addblock.innerHTML = value;         
      resultElement.append(addblock);
      resultElement.setAttribute("style","display:flex;")
  }

  async function sendFile () {  
    const formData = new FormData();
    formData.append("Name", file.files[0].name);
    formData.append("FileBody", file.files[0]);
    let link = "/File/AddFilePhysical/"
    try {   
    const response = await fetch(link,
      {
      method: "POST",
      body: formData
    })
    
    if (response.ok) {
      window.location.href = '/File/Instructions/';
    }
    else{      
      arro(response)
    }

    } catch (error) {
      resultElement.setAttribute("style","display:flex;")
      resultElement.value = error;      
    }   
  }

  document.addEventListener("DOMContentLoaded",setInput);