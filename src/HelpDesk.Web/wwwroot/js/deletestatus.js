var deleteArray = document.querySelectorAll(".Delete_Status");

function check(){
   deleteArray.forEach(
       function (element){                             
       element.addEventListener("click",deleteRequest)}
   );
}

async function deleteRequest(){        
    var link = "/Status/DeleteStatus/"+ this.name;   
    var el = this; 
    const responce = await fetch(link,
    {
      method:"Get"
    }
    )
    .then(responce => responce.json())
        if (responce == "error"){              
      var checkdiv =  el.querySelector(".tooltiptext");
      if(checkdiv == null){              
      let addblock = document.createElement('div')
      addblock.innerHTML = "Удаление статуса не возможно пока есть заявки в этом статусе.";
      // addblock.innerHTML = "You can't delete Status. Because find request whith this status.";
      addblock.className = "tooltiptext";
      addblock.style.display="inline";          
      el.append(addblock);
      
      document.onmouseout = function(e){
        if(addblock){                      
            el.removeChild(addblock);            
            addblock = null;
        }
      }   
     }        
    }
    else{
       window.location.href='/Status/Statuses'
    }
}

document.addEventListener("DOMContentLoaded", check);