export function secondsToHms(d) {
  d = Number(d);
  var h = Math.floor(d / 3600);
  var m = Math.floor((d % 3600) / 60);
  var s = Math.floor((d % 3600) % 60);

  var hDisplay = h > 0 ? h + ":" : "";
  var mDisplay = m > 0 ? m + ":" : "00";
  var sDisplay = s > 0 ? s : "00";

  if (mDisplay.length == 1) {
    mDisplay = `0${mDisplay}`;
  }

  if (sDisplay.length == 1) {
    sDisplay = `0${sDisplay}`;
  }

  return hDisplay + mDisplay + sDisplay;
}
