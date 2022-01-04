export const openLink = (file_path, openInNewTab = false) => {
  var a = document.createElement("A");
  a.href = file_path;
  if(openInNewTab){ a.target = "_blank"; }
  a.download = file_path.substr(file_path.lastIndexOf("/") + 1);
  document.body.appendChild(a);
  a.click();
  document.body.removeChild(a);
};
