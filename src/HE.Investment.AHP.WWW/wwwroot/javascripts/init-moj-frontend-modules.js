MOJFrontend.initAll = function (options) {
  options = typeof options !== 'undefined' ? options : {};
  let scope = typeof options.scope !== 'undefined' ? options.scope : document;

  let $sortableTables = scope.querySelectorAll('[data-module="moj-sortable-table"]');
  MOJFrontend.nodeListForEach($sortableTables, function ($table) {
    new MOJFrontend.SortableTable({
      table: $table
    });
  });
}

MOJFrontend.initAll();
