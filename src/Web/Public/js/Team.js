BigBallz.Teams = {
    setupGrid: function(grid, pager, search) {
        grid.jqGrid({
            url: "Team/List",
            mtype: "post",
            datatype: "json",
            colNames: ['Edit', 'Details', 'TeamID', 'Seleção'],
            colModel: [
                        { name: 'Edit', index: 'Edit', width: 25, align: 'center', sortable: false, hidden: false },
                        { name: 'Details', index: 'Details', width: 25, align: 'center', sortable: false, hidden: false },
                        { name: 'TeamID', index: 'TeamID', width: 50, align: 'center', sortable: false, hidden: true },
                        { name: 'Name', index: 'Name', sortable: false, hidden: false }
                      ],
            rowNum: 10,
            rowList: [10, 20, 50],
            pager: pager,
            sortname: 'TeamID',
            sortorder: "asc",
            viewrecords: true,
            multiselect: false,
            editurl: "Team/Edit",
            width: '100%',
            height: '100%',
            autowidth: true,
            rownumbers: true,
            gridview: true,
            caption: 'Seleção',
            footerrow: false,
            userDataOnFooter: false
        })
        .navGrid(pager, { edit: false, add: false, del: false, search: false, refresh: false });
        }
};

$(document).ready(function () {
    BigBallz.Teams.setupGrid($("#grid"), $("#pager"), $("#search"));
});