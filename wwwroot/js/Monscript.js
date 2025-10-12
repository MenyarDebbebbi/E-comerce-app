$(document).on("click", ".PlusProducts", function (event) {
  event.preventDefault();
  var recordtoupdate = $(this).attr("data-id");
  if (recordtoupdate !== "") {
    $.post("/Panier/PlusProduct", { id: recordtoupdate }, function (data) {
      if (data.ct == "1") {
        $("#totalapayer").text(data.Total);
        $("#quantite_" + recordtoupdate).text(data.Quatite);
        $("#rquantite_" + recordtoupdate).text(data.Quatite);
        $("#total_" + recordtoupdate).text(data.TotalRow);
      }
    }).fail(function () {
      alert("Une erreur s'est produite lors de l'ajout du produit.");
    });
  }
});

$(document).on("click", ".MinProducts", function (event) {
  event.preventDefault();
  var recordtoupdate = $(this).attr("data-id");
  if (recordtoupdate !== "") {
    $.post("/Panier/MinusProduct", { id: recordtoupdate }, function (data) {
      if (data.ct == "1") {
        $("#totalapayer").text(data.Total);
        $("#quantite_" + recordtoupdate).text(data.Quatite);
        $("#rquantite_" + recordtoupdate).text(data.Quatite);
        $("#total_" + recordtoupdate).text(data.TotalRow);
      } else if (data.ct == "0") {
        $("#row-" + recordtoupdate).fadeOut("slow", function () {
          $(this).remove();
          if ($("table tbody tr").length === 0) {
            location.reload();
          }
        });
      }
    }).fail(function () {
      alert("Une erreur s'est produite lors de la diminution de la quantité.");
    });
  }
});

$(document).on("click", ".RemoveLink", function (event) {
  event.preventDefault();
  var recordtoupdate = $(this).attr("data-id");
  if (recordtoupdate != "") {
    if (confirm("Êtes-vous sûr de vouloir supprimer cet article du panier ?")) {
      $.post("/Panier/RemoveProduct", { id: recordtoupdate }, function (data) {
        $("#row-" + recordtoupdate).fadeOut("slow", function () {
          $(this).remove();
          $("#totalapayer").text(data.Total);
          if ($("table tbody tr").length === 0) {
            location.reload();
          }
        });
      }).fail(function () {
        alert("Une erreur s'est produite lors de la suppression du produit.");
      });
    }
  }
});
