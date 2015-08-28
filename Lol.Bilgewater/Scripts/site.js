

function BilgewaterControl() {
    var _this = this;
    this.Init = function (view) {
        if (view == 'Mercs')
            _this.InitMercs();
        if (view == 'Champions')
            _this.InitChampions();
        if (view == 'Items')
            _this.InitItems();
    };
    this.LoadChampionImages = function (toLoad) {        
        _this.Images = [];
        var count = 0;
        var deferred = jQuery.Deferred();
        for (var i = 0; i < toLoad.length; i++) {
            var img = new Image();
            img.id = toLoad[i];
            img.onload = function () {
                count++;
                if (count == toLoad.length) {
                    deferred.resolve();                    
                }
            };
            img.src = '/Image/Champion/' + toLoad[i];
            _this.Images.push(img)
        }
        return deferred;
    };
    this.LoadItemImages = function (toLoad) {
        _this.Images = [];
        var count = 0;
        var deferred = jQuery.Deferred();
        for (var i = 0; i < toLoad.length; i++) {
            var img = new Image();
            img.id = toLoad[i];
            img.onload = function () {
                count++;
                if (count == toLoad.length) {
                    deferred.resolve();
                }
            };
            img.src = '/Image/Item/' + toLoad[i];
            _this.Images.push(img)
        }
        return deferred;
    };
    this.InitChampions = function () {
        _this.LoadChampionChart('TopWinrate');
    };
    this.SelectChampionChart = function (id, title) {
        _this.LoadChampionChart(id);
        $('#championChartSelection').text(title);
    };
    this.LoadChampionChart = function(id){
        $.getJSON('/Data/ChampionChart/' + id, function (response) {
            if (_this.ChampionChart != null)
                _this.ChampionChart.destroy();
            var toLoad = [];
            for (var i = 0; i < response.Champions.length; i++) {
                toLoad.push(response.Champions[i].Key);
            }
            $.when(_this.LoadChampionImages(toLoad)).done(function () {
                var ctx = $('#championChart').get(0).getContext("2d");
                var data = {
                    labels: [],
                    datasets: [
                        {
                            label: "Bilgewater",
                            fillColor: "rgba(200,200,200,0.5)",
                            strokeColor: "rgba(200,200,200,0.8)",
                            highlightFill: "rgba(200,200,200,0.75)",
                            highlightStroke: "rgba(200,200,200,1)",
                            data: []
                        },
                        {
                            label: "Ranked-Solo",
                            fillColor: "rgba(151,187,245,0.5)",
                            strokeColor: "rgba(151,187,245,0.8)",
                            highlightFill: "rgba(151,187,245,0.75)",
                            highlightStroke: "rgba(151,187,245,1)",
                            data: []
                        }
                    ]
                };
                for (var i = 0; i < response.Champions.length; i++) {
                    data.labels.push(response.Champions[i].Key);
                    data.datasets[0].data.push(formatFloat(response.Champions[i].ValueBilgewater));
                    data.datasets[1].data.push(formatFloat(response.Champions[i].ValueRanked));
                }
                var options = {
                    yOffset: -20,
                    scaleLabel: "<%=value%>%",
                    multiTooltipTemplate: "<%= datasetLabel %> - <%= value %>",
                    legendTemplate: '<table class="table table-striped table-hover "><tbody><% for (var i=0; i<datasets.length; i++){%><tr><td style="background-color:<%=datasets[i].fillColor%>"><%if(datasets[i].label){%><%=datasets[i].label%><%}%></td></tr><%}%></tbody></table>'
                };
                _this.ChampionChart = new Chart(ctx).Bar(data, options);
                $('#championLegend').html(_this.ChampionChart.generateLegend());
            });
        });
    }
    this.SelectItemChart = function (id, title) {
        _this.LoadItemChart(id);
        $('#itemChartSelection').text(title);
    };
    this.LoadItemChart = function (id) {
        $.getJSON('/Data/ItemChart/' + id, function (response) {
            if (_this.ItemChart != null)
                _this.ItemChart.destroy();
            var toLoad = [];
            for (var i = 0; i < response.Items.length; i++) {
                toLoad.push(response.Items[i].Id);
            }
            $.when(_this.LoadItemImages(toLoad)).done(function () {
                var ctx = $('#itemChart').get(0).getContext("2d");
                var data = {
                    labels: [],
                    datasets: [
                        {
                            label: "Bilgewater",
                            fillColor: "rgba(200,200,200,0.5)",
                            strokeColor: "rgba(200,200,200,0.8)",
                            highlightFill: "rgba(200,200,200,0.75)",
                            highlightStroke: "rgba(200,200,200,1)",
                            data: []
                        },
                        {
                            label: "Ranked-Solo",
                            fillColor: "rgba(151,187,245,0.5)",
                            strokeColor: "rgba(151,187,245,0.8)",
                            highlightFill: "rgba(151,187,245,0.75)",
                            highlightStroke: "rgba(151,187,245,1)",
                            data: []
                        }
                    ]
                };
                for (var i = 0; i < response.Items.length; i++) {
                    data.labels.push(response.Items[i].Id);
                    data.datasets[0].data.push(formatFloat(response.Items[i].ValueBilgewater));
                    data.datasets[1].data.push(formatFloat(response.Items[i].ValueRanked));
                }
                var options = {
                    yOffset: -20,
                    scaleLabel: "<%=value%>%",
                    multiTooltipTemplate: "<%= datasetLabel %> - <%= value %>",
                    legendTemplate: '<table class="table table-striped table-hover "><tbody><% for (var i=0; i<datasets.length; i++){%><tr><td style="background-color:<%=datasets[i].fillColor%>"><%if(datasets[i].label){%><%=datasets[i].label%><%}%></td></tr><%}%></tbody></table>'
                };
                _this.ItemChart = new Chart(ctx).Bar(data, options);
                $('#itemLegend').html(_this.ItemChart.generateLegend());
            });
        });
    }
    this.InitItems = function () {
        _this.LoadItemChart('TopWinrate');
    };
    this.InitMercs = function () {
        $.when(_this.LoadItemImages([3611, 3612, 3613, 3614, 3070])).done(function () {
            $.getJSON('/Data/Role', function (response) {
                var ctx = $('#roleStats').get(0).getContext("2d");
                var data = {
                    labels: ['Tank', 'Mage', 'Marksman', 'Fighter', 'Support', 'Assassin'],
                    datasets: [
                        {
                            label: "Bilgwater",
                            fillColor: "rgba(99,255,38,0.5)",
                            strokeColor: "rgba(99,255,38,0.8)",
                            highlightFill: "rgba(99,255,38,0.75)",
                            highlightStroke: "rgba(99,255,38,1)",
                            data: []
                        },
                        {
                            label: "Ranked-Solo",
                            fillColor: "rgba(255,135,197,0.5)",
                            strokeColor: "rgba(255,135,197,0.8)",
                            highlightFill: "rgba(255,135,197,0.75)",
                            highlightStroke: "rgba(255,135,197,1)",
                            data: []
                        }
                    ]
                };
                data.datasets[0].data.push(formatRole(response.Bilgewater['Tank'].Winrate));
                data.datasets[0].data.push(formatRole(response.Bilgewater['Mage'].Winrate));
                data.datasets[0].data.push(formatRole(response.Bilgewater['Marksman'].Winrate));
                data.datasets[0].data.push(formatRole(response.Bilgewater['Fighter'].Winrate));
                data.datasets[0].data.push(formatRole(response.Bilgewater['Support'].Winrate));
                data.datasets[0].data.push(formatRole(response.Bilgewater['Assassin'].Winrate));

                data.datasets[1].data.push(formatRole(response.Ranked['Tank'].Winrate));
                data.datasets[1].data.push(formatRole(response.Ranked['Mage'].Winrate));
                data.datasets[1].data.push(formatRole(response.Ranked['Marksman'].Winrate));
                data.datasets[1].data.push(formatRole(response.Ranked['Fighter'].Winrate));
                data.datasets[1].data.push(formatRole(response.Ranked['Support'].Winrate));
                data.datasets[1].data.push(formatRole(response.Ranked['Assassin'].Winrate));

                var options = {
                    multiTooltipTemplate: "<%= datasetLabel %> - <%= value %>",
                    scaleLabel: "<%=value%> %",
                    legendTemplate: '<table class="table table-striped table-hover "><tbody><% for (var i=0; i<datasets.length; i++){%><tr><td style="background-color:<%=datasets[i].fillColor%>"><%if(datasets[i].label){%><%=datasets[i].label%><%}%></td></tr><%}%></tbody></table>'
                };
                var myBarChart = new Chart(ctx).Bar(data, options);
                $('#roleLegend').html(myBarChart.generateLegend());
            }),
            $.getJSON('/Data/Duration', function (response) {
                var ctx = $('#durationStats').get(0).getContext("2d");
                var data = {
                    labels: ['Matchduration', 'Firstblood', 'First Tower', 'First Drake', 'First Inhibitor', 'First Baron'],
                    datasets: [
                        {
                            label: "Bilgwater",
                            fillColor: "rgba(255,112,56,0.5)",
                            strokeColor: "rgba(255,112,56,0.8)",
                            highlightFill: "rgba(255,112,56,0.75)",
                            highlightStroke: "rgba(255,112,56,1)",
                            data: []
                        },
                        {
                            label: "Ranked-Solo",
                            fillColor: "rgba(104,164,255,0.5)",
                            strokeColor: "rgba(104,164,255,0.8)",
                            highlightFill: "rgba(104,164,255,0.75)",
                            highlightStroke: "rgba(104,164,255,1)",
                            data: []
                        }
                    ]
                };
                data.datasets[0].data.push(formatDuration(response.Bilgewater.AvgMatchDuration));
                data.datasets[0].data.push(formatDuration(response.Bilgewater.AvgFirstblood));
                data.datasets[0].data.push(formatDuration(response.Bilgewater.AvgFirstTower));
                data.datasets[0].data.push(formatDuration(response.Bilgewater.AvgFirstDragon));
                data.datasets[0].data.push(formatDuration(response.Bilgewater.AvgFirstInhib));
                data.datasets[0].data.push(formatDuration(response.Bilgewater.AvgFirstBaron));

                data.datasets[1].data.push(formatDuration(response.Ranked.AvgMatchDuration));
                data.datasets[1].data.push(formatDuration(response.Ranked.AvgFirstblood));
                data.datasets[1].data.push(formatDuration(response.Ranked.AvgFirstTower));
                data.datasets[1].data.push(formatDuration(response.Ranked.AvgFirstDragon));
                data.datasets[1].data.push(formatDuration(response.Ranked.AvgFirstInhib));
                data.datasets[1].data.push(formatDuration(response.Ranked.AvgFirstBaron));

                var options = {
                    multiTooltipTemplate: "<%= datasetLabel %> - <%= value %>",
                    scaleLabel: "<%=value%> min",
                    legendTemplate: '<table class="table table-striped table-hover "><tbody><% for (var i=0; i<datasets.length; i++){%><tr><td style="background-color:<%=datasets[i].fillColor%>"><%if(datasets[i].label){%><%=datasets[i].label%><%}%></td></tr><%}%></tbody></table>'
                };
                var myBarChart = new Chart(ctx).Bar(data, options);
                $('#durationLegend').html(myBarChart.generateLegend());
            }),
            $.getJSON('/Data/Mercs', function (response) {
                var ctx = $('#mercPopularity').get(0).getContext("2d");
                var data = {
                    labels: [],
                    datasets: [
                        {
                            label: "Pickrate",
                            fillColor: "rgba(200,200,200,0.5)",
                            strokeColor: "rgba(200,200,200,0.8)",
                            highlightFill: "rgba(200,200,200,0.75)",
                            highlightStroke: "rgba(200,200,200,1)",
                            data: []
                        },
                        {
                            label: "Winrate",
                            fillColor: "rgba(151,187,245,0.5)",
                            strokeColor: "rgba(151,187,245,0.8)",
                            highlightFill: "rgba(151,187,245,0.75)",
                            highlightStroke: "rgba(151,187,245,1)",
                            data: []
                        }
                    ]
                };
                for (var i = 0; i < response.Stats.length; i++) {
                    data.labels.push(response.Stats[i].Id);
                    data.datasets[0].data.push(response.Stats[i].Pickrate);
                    data.datasets[1].data.push(response.Stats[i].Winrate * 2);
                }
                var options = {
                    yOffset: -20,
                    scaleLabel: "<%=value%>%",
                    multiTooltipTemplate: "<%= datasetLabel %> - <%= value %>",
                    legendTemplate: '<table class="table table-striped table-hover "><tbody><% for (var i=0; i<datasets.length; i++){%><tr><td style="background-color:<%=datasets[i].fillColor%>"><%if(datasets[i].label){%><%=datasets[i].label%><%}%></td></tr><%}%></tbody></table>'
                };
                var myBarChart = new Chart(ctx).Bar(data, options);
                $('#mercLegend').html(myBarChart.generateLegend());
                var descPanel = $('#mercs');
                for (var i = 0; i < response.Infos.length; i++) {
                    var elem = $('<div class="merc-description"><img class="img-rounded" src="/Image/Item/' + response.Infos[i].Id + '" title="merc" /><div class="desc">' + response.Infos[i].Name + '</div><div class="desc">' + response.Infos[i].Description + '</div></div>');
                    descPanel.append(elem);
                }
                $('font', descPanel).remove();
                descPanel.show();
            });
        });
    };
    this.ShowItemDetails = function (id) {
        $.getJSON('/Data/Item/' + id, function (data) {
            $('#dialogTitle').text(data.Description.Name);
            $('.label-success, .label-danger').removeClass('label-success').removeClass('label-danger');
            if (data.Ranked != null) {
                if (data.Bilgewater.Winrate > data.Ranked.Winrate) {
                    $('#wrBilgewater').addClass('label-success');
                    $('#wrRanked').addClass('label-danger');
                }
                else {
                    $('#wrBilgewater').addClass('label-danger');
                    $('#wrRanked').addClass('label-success');
                }
                $('#wrBilgewater').text(formatWin(data.Bilgewater.Winrate) + '%');
                $('#wrRanked').text(formatWin(data.Ranked.Winrate) + '%');

                if (data.Bilgewater.Pickrate > data.Ranked.Pickrate) {
                    $('#pickBilgewater').addClass('label-success');
                    $('#pickRanked').addClass('label-danger');
                }
                else {
                    $('#pickBilgewater').addClass('label-danger');
                    $('#pickRanked').addClass('label-success');
                }
                $('#pickBilgewater').text(formatFloat(data.Bilgewater.Pickrate) + '%');
                $('#pickRanked').text(formatFloat(data.Ranked.Pickrate) + '%');
            }
            else {
                $('#wrBilgewater').text(formatWin(data.Bilgewater.Winrate) + '%');
                $('#wrRanked').text('N/A');
                $('#pickBilgewater').text(formatFloat(data.Bilgewater.Pickrate) + '%');
                $('#pickRanked').text('N/A');
            }
            $('#itemDesc').html(data.Description.Description);
        });
    }
    this.ShowChampDetails = function (id, key) {
        $.getJSON('/Data/Champion/'+key, function (data) {
            $('#dialogTitle').text(data.Description.Name + ' - ' + data.Description.Title);
            $('#dialogBody').css('background-image', 'url(/Image/Splash/' + key + ')');
            $('.label-success, .label-danger').removeClass('label-success').removeClass('label-danger');

            if (data.Bilgewater.Winrate > data.Ranked.Winrate) {
                $('#wrBilgewater').addClass('label-success');
                $('#wrRanked').addClass('label-danger');
            }
            else {
                $('#wrBilgewater').addClass('label-danger');
                $('#wrRanked').addClass('label-success');
            }
            $('#wrBilgewater').text(formatWin(data.Bilgewater.Winrate) + '%');
            $('#wrRanked').text(formatWin(data.Ranked.Winrate) + '%');

            if (data.Bilgewater.Pickrate > data.Ranked.Pickrate) {
                $('#pickBilgewater').addClass('label-success');
                $('#pickRanked').addClass('label-danger');
            }
            else {
                $('#pickBilgewater').addClass('label-danger');
                $('#pickRanked').addClass('label-success');
            }
            $('#pickBilgewater').text(formatFloat(data.Bilgewater.Pickrate) + '%');
            $('#pickRanked').text(formatFloat(data.Ranked.Pickrate) + '%');

            if (data.Bilgewater.AvgKillCount > data.Ranked.AvgKillCount) {
                $('#killsBilgewater').addClass('label-success');
                $('#killsRanked').addClass('label-danger');
            }
            else {
                $('#killsBilgewater').addClass('label-danger');
                $('#killsRanked').addClass('label-success');
            }
            $('#killsBilgewater').text(formatFloat(data.Bilgewater.AvgKillCount));
            $('#killsRanked').text(formatFloat(data.Ranked.AvgKillCount));

            if (data.Bilgewater.AvgDeathCount > data.Ranked.AvgDeathCount) {
                $('#deathBilgewater').addClass('label-success');
                $('#deahtRanked').addClass('label-danger');
            }
            else {
                $('#deathBilgewater').addClass('label-danger');
                $('#deahtRanked').addClass('label-success');
            }
            $('#deathBilgewater').text(formatFloat(data.Bilgewater.AvgDeathCount));
            $('#deahtRanked').text(formatFloat(data.Ranked.AvgDeathCount));

            if (data.Bilgewater.AvgAssistCount > data.Ranked.AvgAssistCount) {
                $('#assistsBilgewater').addClass('label-success');
                $('#assistsRanked').addClass('label-danger');
            }
            else {
                $('#assistsBilgewater').addClass('label-danger');
                $('#assistsRanked').addClass('label-success');
            }
            $('#assistsBilgewater').text(formatFloat(data.Bilgewater.AvgAssistCount));
            $('#assistsRanked').text(formatFloat(data.Ranked.AvgAssistCount));

            if (data.Bilgewater.AvgMinionCount + data.Bilgewater.AvgJungleCount > data.Ranked.AvgMinionCount + data.Ranked.AvgJungleCount) {
                $('#csBilgewater').addClass('label-success');
                $('#csRanked').addClass('label-danger');
            }
            else {
                $('#csBilgewater').addClass('label-danger');
                $('#csRanked').addClass('label-success');
            }
            $('#csBilgewater').text(formatFloat(data.Bilgewater.AvgMinionCount + data.Bilgewater.AvgJungleCount));
            $('#csRanked').text(formatFloat(data.Ranked.AvgMinionCount + data.Ranked.AvgJungleCount));
        });
    }
}

function formatDuration(x) {
    return Number((x / 60).toFixed(2))
}

function formatRole(x) {
    return Number((x * 100).toFixed(2))
}

function formatWin(x) {
    return Number((x * 100).toFixed(2))
}

function formatFloat(x) {
    return Number((x).toFixed(2))
}

function toDateTime(secs) {
    var t = new Date(1970, 0, 1);
    t.setSeconds(secs);
    return t;
}