

function ItemsetControl() {
    var _this = this;
    this.Init = function () {
        _this.CurrentMap = 'SR';
        _this.CurrentChamp = 'Aatrox';
        $('[data-toggle="popover"]').popover();
        $('#champFilter').keyup(function () {
            _this.FilterChamps();
        });
        $('.champ').click(_this.SelectChamp);
        $('.select-itemset').click(_this.SelectItemset);
        $('.new-itemset').click(_this.NewItemset);
        _this.FilterChamps();
    };
    this.FilterChamps = function () {
        var search = $('#champFilter').val().toLowerCase();
        $('.champ').each(function () {
            var name = $(this).attr('rel');
            if (name.indexOf(search) >= 0)
                $(this).show();
            else
                $(this).hide();
        });
    };
    this.SelectChamp = function(){
        _this.CurrentChamp = $(this).attr('rel');
        $('#champ-list a.active').removeClass('active');
        $('#champ-list a[rel=' + _this.CurrentChamp + ']').addClass('active');
        
    };
    this.SelectMap = function (key) {
        _this.CurrentMap = key;
        $('#map-group a.active').removeClass('active');
        $('#map-group a[rel=' + key + ']').addClass('active');
    };
    this.SelectItemset = function () {
        var name = $(this).attr('name');
        $.getJSON('/Data/LoadItemset/' + name + '?champ=' + _this.CurrentChamp + '&map=' + _this.CurrentMap, function (response) {
            _this.CurrentSet = response;
            $('#itemsetName').val(_this.CurrentSet.Title);            
            if (name == "recommanded") {
                _this.RenderItemset(_this.CurrentSet, true);
            }
            else {
                _this.RenderItemset(_this.CurrentSet, false);
            }
        });
    };
    this.RenderItemset = function (set, readonly) {

    };
}