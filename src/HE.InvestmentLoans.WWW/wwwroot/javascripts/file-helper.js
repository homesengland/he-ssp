function onFileChange() {
  var myForm = document.getElementById('formWithFile');
  myForm.submit();
}

document.getElementById("File").addEventListener("change", onFileChange);
