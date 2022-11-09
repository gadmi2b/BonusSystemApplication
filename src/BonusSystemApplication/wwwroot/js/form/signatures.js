$(document).ready(function () {
  $('.js-signature').change(function (event) {
    
    //event.target.id - id of changed checkbox
    //event.target.value - initial value of changed checkbox

    // TODO: call ajax request and send: id and initial value of checkbox
    //       all signature block should be locked until server responce
    //       responce should contain: success, id of affected signature block, signature (if target.value == false)
    //                                         id and string.empty signature             (if target.value == true) --- !need to drop reject also!
    //                                error,   message, => return checkbox value back

    //       (in the case of reject): success, id of affected signature block, signature (if reject was before main signature) (if target.value == false)
    //       (in the case of reject): success, id = string.empty (do nothing)            (if reject was after main signature)  (if target.value == false)
    //       (in the case of reject): success, id = string.empty (do nothing)            (in any cased) (if target.value == true)
    //                                error,   message, => return checkbox value back

    //

  });
});