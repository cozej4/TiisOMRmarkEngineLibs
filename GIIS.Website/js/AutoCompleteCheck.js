 var isresult = "test";
function pageLoad(sender, args) {

    $find('autocomplteextender1')._onMethodComplete = function (result, context) {

        $find('autocomplteextender1')._update(context, result, /* cacheResults */false);
        webservice_callback(result, context, sender);

    };

}
function webservice_callback(result, context, sender) {

    if (result == "") {
        $find("autocomplteextender1").get_element().style.backgroundColor = "red";
        isresult = "";
    }
    else {
        $find("autocomplteextender1").get_element().style.backgroundColor = "white";
        isresult = "test";

    }
}
function checkHFacility() {
    if (isresult == "") {
        alert("Please choose a health facility from the list!");
        // alert(text);
        return false;
    }
    return true;
}
