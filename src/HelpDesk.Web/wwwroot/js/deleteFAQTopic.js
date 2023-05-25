var deleteArray = document.querySelectorAll(".Delete_Topic");

function check(){
   deleteArray.forEach(
       function (element){                             
       element.addEventListener("click",deleteRequest)}
   );
}

async function deleteRequest(){        
    var link = "/FAQ/DeleteFAQTopic/"+ this.name;   
    var el = this; 
    const responce = await fetch(link,
    {
      method:"Get"
    }
    )
    .then(responce => responce.json())
        if (responce == "error"){              
      var checkdiv =  el.querySelector(".tooltiptextfaq");
      if(checkdiv == null){              
      let addblock = document.createElement('div')
      addblock.innerHTML = "Удаление не возможно пока есть вложенные стати в этой теме.";
      // addblock.innerHTML = "You can't delete this topic with notes, clear notes first.";
      addblock.className = "tooltiptextfaq";
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
       window.location.href='/FAQ/FAQTopics'
    }
}

document.addEventListener("DOMContentLoaded", check);