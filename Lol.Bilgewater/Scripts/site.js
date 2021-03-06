﻿

function BilgewaterControl() {
    var _this = this;
    this.Init = function (view, onlyBilgewater) {
        // one init page for each chart page
        _this.OnlyBilgewater = onlyBilgewater;
        if (view == 'Mercs')
            _this.InitMercs();
        if (view == 'Champions')
            _this.InitChampions();
        if (view == 'Items')
            _this.InitItems();
    };
    // helper method for loading champion icons
    this.LoadChampionImages = function (toLoad) {        
        _this.Images = [];
        var count = 0;
        // create deferred so chart creation can wait until images are loaded
        var deferred = jQuery.Deferred();
        for (var i = 0; i < toLoad.length; i++) {
            var img = new Image();
            img.id = toLoad[i];
            img.onload = function () {
                count++;
                // onload counts loads
                // resolves deferred when all images are loaded
                if (count == toLoad.length) {
                    deferred.resolve();                    
                }
            };
            img.src = '/Image/Champion/' + toLoad[i];
            // safe image in array
            _this.Images.push(img)
        }
        return deferred;
    };

    // helper method for loading ietem icons
    this.LoadItemImages = function (toLoad) {
        _this.Images = [];
        var count = 0;
        // create deferred so chart creation can wait until images are loaded
        var deferred = jQuery.Deferred();
        for (var i = 0; i < toLoad.length; i++) {
            var img = new Image();
            img.id = toLoad[i];
            img.onload = function () {
                count++;
                // onload counts loads
                // resolves deferred when all images are loaded
                if (count == toLoad.length) {
                    deferred.resolve();
                }
            };
            img.src = '/Image/Item/' + toLoad[i];
            // safe image in array
            _this.Images.push(img)
        }
        return deferred;
    };
    this.InitChampions = function () {
        _this.LoadChampionChart('TopWinrate');
    };
    // method is called from chart selection dropdown
    this.SelectChampionChart = function (id, title) {
        _this.LoadChampionChart(id);
        // update dropdown selected span
        $('#championChartSelection').text(title);
    };
    this.LoadChampionChart = function(id){
        $.getJSON('/Data/ChampionChart/' + id, function (response) {
            // destroy previous chart
            if (_this.ChampionChart != null)
                _this.ChampionChart.destroy();
            var toLoad = [];
            // select champion images to load
            for (var i = 0; i < response.Champions.length; i++) {
                toLoad.push(response.Champions[i].Key);
            }
            // get images and wait for them
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
                // format for y axis legend
                var format = response.Format;                
                // fill dataset for  chart
                for (var i = 0; i < response.Champions.length; i++) {
                    data.labels.push(response.Champions[i].Key);
                    // format by selected format 
                    if (response.Format == 'Win') {
                        data.datasets[0].data.push(formatWin(response.Champions[i].ValueBilgewater));
                        data.datasets[1].data.push(formatWin(response.Champions[i].ValueRanked));
                    }
                    else if (response.Format == 'Int') {
                        data.datasets[0].data.push(formatInt(response.Champions[i].ValueBilgewater));
                        data.datasets[1].data.push(formatInt(response.Champions[i].ValueRanked));
                    }
                    else {
                        data.datasets[0].data.push(formatFloat(response.Champions[i].ValueBilgewater));
                        data.datasets[1].data.push(formatFloat(response.Champions[i].ValueRanked));
                    }
                }
                var options = {
                    yOffset: -20,
                    scaleLabel: format == 'Int' ? "<%=value%>" : "<%=value%>%",
                    multiTooltipTemplate: "<%= datasetLabel %> - <%= value %>",
                    legendTemplate: '<table class="table table-striped table-hover "><tbody><% for (var i=0; i<datasets.length; i++){%><tr><td style="background-color:<%=datasets[i].fillColor%>"><%if(datasets[i].label){%><%=datasets[i].label%><%}%></td></tr><%}%></tbody></table>'
                };
                // chart creation
                _this.ChampionChart = new Chart(ctx).Bar(data, options);
                // render legend
                $('#championLegend').html(_this.ChampionChart.generateLegend());
            });
        });
    }
    // method is called from chart selection dropdown
    this.SelectItemChart = function (id, title) {
        _this.LoadItemChart(id);
        // update dropdown selected span
        $('#itemChartSelection').text(title);
    };
    this.LoadItemChart = function (id) {
        $.getJSON('/Data/ItemChart/' + id + '?onlyBilgewater=' + _this.OnlyBilgewater, function (response) {
            // destroy previous chart
            if (_this.ItemChart != null)
                _this.ItemChart.destroy();
            var toLoad = [];
            // select champion images to load
            for (var i = 0; i < response.Items.length; i++) {
                toLoad.push(response.Items[i].Id);
            }
            // get images and wait for them
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
                // fill dataset for  chart
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
                // chart creation
                _this.ItemChart = new Chart(ctx).Bar(data, options);
                // get images and wait for them
                $('#itemLegend').html(_this.ItemChart.generateLegend());
            });
        });
    }
    this.InitItems = function () {
        _this.LoadItemChart('TopWinrate');
    };
    this.InitMercs = function () {
        // get images and wait for them
        $.when(_this.LoadItemImages([3611, 3612, 3613, 3614, 3070])).done(function () {
            $.getJSON('/Data/Role', function (response) {
                var ctx = $('#roleStats').get(0).getContext("2d");
                var data = {
                    labels: [Strings['Tank'], Strings['Mage'], Strings['Marksman'], Strings['Fighter'], Strings['Support'], Strings['Assassin']],
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
                // fill dataset for bilgewater
                data.datasets[0].data.push(formatRole(response.Bilgewater['Tank'].Winrate));
                data.datasets[0].data.push(formatRole(response.Bilgewater['Mage'].Winrate));
                data.datasets[0].data.push(formatRole(response.Bilgewater['Marksman'].Winrate));
                data.datasets[0].data.push(formatRole(response.Bilgewater['Fighter'].Winrate));
                data.datasets[0].data.push(formatRole(response.Bilgewater['Support'].Winrate));
                data.datasets[0].data.push(formatRole(response.Bilgewater['Assassin'].Winrate));
                // fill dataset for ranked
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
                // create chart
                var myBarChart = new Chart(ctx).Bar(data, options);
                // create legend
                $('#roleLegend').html(myBarChart.generateLegend());
            }),
            $.getJSON('/Data/Duration', function (response) {
                var ctx = $('#durationStats').get(0).getContext("2d");
                var data = {
                    labels: [Strings['Matchduration'], Strings['FirstBlood'], Strings['FirstTower'], Strings['FirstDrake'], Strings['FirstInhibitor'], Strings['FirstBaron']],
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
                // fill dataset for bilgewater
                data.datasets[0].data.push(formatDuration(response.Bilgewater.AvgMatchDuration));
                data.datasets[0].data.push(formatDuration(response.Bilgewater.AvgFirstblood));
                data.datasets[0].data.push(formatDuration(response.Bilgewater.AvgFirstTower));
                data.datasets[0].data.push(formatDuration(response.Bilgewater.AvgFirstDragon));
                data.datasets[0].data.push(formatDuration(response.Bilgewater.AvgFirstInhib));
                data.datasets[0].data.push(formatDuration(response.Bilgewater.AvgFirstBaron));
                // fill dataset for ranked
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
                // create chart
                var myBarChart = new Chart(ctx).Bar(data, options);
                // create legend
                $('#durationLegend').html(myBarChart.generateLegend());
            }),
            $.getJSON('/Data/Mercs', function (response) {
                var ctx = $('#mercPopularity').get(0).getContext("2d");
                var data = {
                    labels: [],
                    datasets: [
                        {
                            label: Strings["Pickrate"],
                            fillColor: "rgba(200,200,200,0.5)",
                            strokeColor: "rgba(200,200,200,0.8)",
                            highlightFill: "rgba(200,200,200,0.75)",
                            highlightStroke: "rgba(200,200,200,1)",
                            data: []
                        },
                        {
                            label: Strings["Winrate"],
                            fillColor: "rgba(151,187,245,0.5)",
                            strokeColor: "rgba(151,187,245,0.8)",
                            highlightFill: "rgba(151,187,245,0.75)",
                            highlightStroke: "rgba(151,187,245,1)",
                            data: []
                        }
                    ]
                };
                // fill dataset
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
                // create chart
                var myBarChart = new Chart(ctx).Bar(data, options);
                // create legend
                $('#mercLegend').html(myBarChart.generateLegend());
                // create merc description panel
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
        // get item details from server
        $.getJSON('/Data/Item/' + id, function (data) {
            $('#dialogTitle').text(data.Description.Name);
            // fill elements - here would be a template engine a better solution
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
        // get item Champion from server
        $.getJSON('/Data/Champion/'+key, function (data) {
            $('#dialogTitle').text(data.Description.Name + ' - ' + data.Description.Title);
            $('#dialogBody').css('background-image', 'url(/Image/Splash/' + key + ')');
            $('.label-success, .label-danger').removeClass('label-success').removeClass('label-danger');
            // fill elements - here would be a template engine a better solution
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

function formatInt(x) {
    return Number((x).toFixed(0))
}

function toDateTime(secs) {
    var t = new Date(1970, 0, 1);
    t.setSeconds(secs);
    return t;
}