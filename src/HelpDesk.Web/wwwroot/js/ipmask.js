
var ipv4_address = document.getElementsByName("Ip")

 function createMask(){
     var im = new Inputmask("9{1,3}.9{1,3}.9{1,3}.9{1,3}")
     im.mask(ipv4_address);
 }

  document.addEventListener("DOMContentLoaded", createMask);