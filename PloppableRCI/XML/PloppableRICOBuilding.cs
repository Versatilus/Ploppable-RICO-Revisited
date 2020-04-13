﻿using System;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;


namespace PloppableRICO
{
    [XmlType( "Building" )]
    public class RICOBuilding : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public RICOBuilding()
        {
            dbKey = 0;
            _workplaces = new int[] { 0, 0, 0, 0 };
            _lastSetWorkplaces = new int[] { 0, 0, 0, 0 };
            workplaceDeviationString = "";
            workplaceDeviation = new int[] { 0, 0, 0, 0 };
            _lastSetDeviations = new int[] { 0, 0, 0, 0 };
            name = "";
            author = "";
            story = "";
            steamId = "";
            service = "";
            subService = "";
            constructionCost = 10;
            uiCategory = "";
            homeCount = 0;
            level = 0;
            pollutionRadius = 0;

            density = 0;

            fireHazard = 0;
            fireTolerance = 0;
            fireSize = 255;
            popbalanceEnabled = true;
            ricoEnabled = true;
            educationRatioEnabled = false;
            pollutionEnabled = true;
            manualWorkerEnabled = true;
            manualHomeEnabled = true;
            constructionCostEnabled = true;
            RealityIgnored = false;
            _isDirty = false;
        }

        public void HandleSetterEvents( bool changed )
        {
            if ( !changed || parent == null )
                return;

            parent.RaiseBuildingPropertyChanged( this );
            var d = _isDirty;
            _isDirty = true;
            if ( !d )
                parent.RaiseBuildingDirtynessChanged( this );
        }


        private Regex RegexXmlIntegerValue = new System.Text.RegularExpressions.Regex("^ *(\\d+) *$");
        private Regex RegexXML4IntegerValues = new System.Text.RegularExpressions.Regex("^ *(\\d+) *, *(\\d+) *, *(\\d+) *, *(\\d+) *");

        [XmlAttribute( "name" )]
        public string name
        {
            get { return _name; }
            set { var v = _name; _name = value; HandleSetterEvents( value != v ); }
        }
        string _name;

        [DefaultValue( 0 )]
        [XmlAttribute( "usages" )]
        public int usages { get { return _usages; } set { var v = _usages; _usages = value; HandleSetterEvents( v != value ); } }
        int _usages;

        [XmlAttribute( "author" )]
        [DefaultValue( "" )]
        public string author { get { return _author; } set { var v = _author; _author = value; HandleSetterEvents( v != value ); } }
        string _author;

        [XmlElement( "story" )]
        [DefaultValue( "" )]
        public string story { get { return _story; } set { var v = _story; _story = value; HandleSetterEvents( v != value ); } }
        string _story;

        [XmlAttribute( "service" )]
        public string service
        {
            get { return _service; }
            set { var v = _service; _service = value; HandleSetterEvents( value != v ); }
        }
        string _service;

        [XmlAttribute("density")]
        public int density
        {
            get { return _density; }
            set { var v = _density; _density = value; HandleSetterEvents(value != v); }
        }
        int _density;

        [XmlAttribute( "sub-service" )]
        public string subService
        {
            get { return _subService; }
            set
            {
                var v = _subService; _subService = value; HandleSetterEvents( v != value );
            }
        }
        string _subService;

        [XmlAttribute( "construction-cost" )]
        public int constructionCost
        {
            get
            {
                // Enforce minimum construction cost of 10 for compatability with other mods (e.g. Real Time, Realistic Construction)
                _constructionCost = Math.Max(_constructionCost, 10);
                return _constructionCost;
            }
            set
            {
                var v = _constructionCost;
                _constructionCost = value;
                HandleSetterEvents( v != value );
            }
        }
        int _constructionCost;

        private string _UICategory;

        [XmlAttribute( "ui-category" )]
        public string uiCategory
        {
            get { return _UICategory != "" ? _UICategory : Util.UICategoryOf( service, subService ); }
            set { _UICategory = value; }
        }

        [XmlAttribute( "homes" )]
        [DefaultValue( 0 )]
        public int homeCount
        {
            get { return _homeCount; }
            set
            {
                var v = _homeCount; _homeCount = value; HandleSetterEvents( v != value );
            }
        }
        int _homeCount;

        [XmlAttribute( "steam-id" )]
        public string steamId { get { return _steamId; } set { var v = _steamId; _steamId = value; HandleSetterEvents( v != value ); } }
        string _steamId;

        private int _level;
        [XmlAttribute( "level" )]
        public int level
        {
            get
            {
                return _level;
            }
            set
            {
                var v = _level; _level = value; HandleSetterEvents( v != value );
            }
        }

        //Pollution
        [DefaultValue( 0 )]
        [XmlAttribute( "pollution-radius" )]
        public int pollutionRadius { get { return _pollutionRadius; } set { var v = _pollutionRadius; _pollutionRadius = value; HandleSetterEvents( v != value ); } }
        int _pollutionRadius;

        [DefaultValue( 0 )]
        [XmlAttribute( "db-key" )]
        public int dbKey { get { return _dbKey; } set { var v = _dbKey; _dbKey = value; HandleSetterEvents( v != value ); } }
        int _dbKey;

        [DefaultValue( 0 )]
        [XmlAttribute( "fire-hazard" )]
        public int fireHazard { get { return _fireHazard; } set { var v = _fireHazard; _fireHazard = value; HandleSetterEvents( v != value ); } }
        int _fireHazard;

        [DefaultValue( 255 )]
        [XmlAttribute( "fire-size" )]
        public int fireSize
        {
            get { return _fireSize; }
            set
            {
                var v = _fireSize; _fireSize = value; HandleSetterEvents( v != value );
            }
        }
        int _fireSize;

        [DefaultValue( 0 )]
        [XmlAttribute( "fire-tolerance" )]
        public int fireTolerance { get { return _fireTolerance; } set { var v = _fireTolerance; _fireTolerance = value; HandleSetterEvents( v != value ); } }
        int _fireTolerance;

        //Toggles
        [DefaultValue( true )]
        [XmlAttribute( "enable-popbalance" )]
        public bool popbalanceEnabled { get { return _popbalanceEnabled; } set { var v = _popbalanceEnabled; _popbalanceEnabled = value; HandleSetterEvents( v != value ); } }
        bool _popbalanceEnabled;

        [DefaultValue( true )]
        [XmlAttribute( "enable-rico" )]
        public bool ricoEnabled { get { return _ricoEnabled; } set { var v = _ricoEnabled; _ricoEnabled = value; HandleSetterEvents( v != value ); } }
        bool _ricoEnabled;

        [DefaultValue(false)]
        [XmlAttribute("growable")]
        public bool growable { get { return _growable; } set { var v = _growable; _growable = value; HandleSetterEvents(v != value); } }
        bool _growable;

        [DefaultValue( false )]
        [XmlAttribute( "enable-educationratio" )]
        public bool educationRatioEnabled { get { return _educationRatioEnabled; } set { var v = _educationRatioEnabled; _educationRatioEnabled = value; HandleSetterEvents( v != value ); } }
        bool _educationRatioEnabled;

        [DefaultValue( true )]
        [XmlAttribute( "enable-pollution" )]
        public bool pollutionEnabled { get { return _pollutionEnabled; } set { var v = _pollutionEnabled; _pollutionEnabled = value; HandleSetterEvents( v != value ); } }
        bool _pollutionEnabled;

        [DefaultValue( true )]
        [XmlAttribute( "enable-workercount" )]
        public bool manualWorkerEnabled { get { return _manualWorkerEnabled; } set { var v = _manualWorkerEnabled; _manualWorkerEnabled = value; HandleSetterEvents( v != value ); } }
        bool _manualWorkerEnabled;

        [DefaultValue( true )]
        [XmlAttribute( "enable-homecount" )]
        public bool manualHomeEnabled { get { return _manualHomeEnabled; } set { var v = _manualHomeEnabled; _manualHomeEnabled = value; HandleSetterEvents( v != value ); } }
        bool _manualHomeEnabled;

        [DefaultValue( true )]
        [XmlAttribute( "enable-constructioncost" )]
        public bool constructionCostEnabled { get { return _constructionCostEnabled; } set { var v = _constructionCostEnabled; _constructionCostEnabled = value; HandleSetterEvents( v != value ); } }
        bool _constructionCostEnabled;

        [XmlAttribute( "workplaces" )]
        [DefaultValue( "0,0,0,0" )]
        public string workplacesString
        {
            get
            {
                if ( workplaceCount == 0 )
                    return "0,0,0,0";

                return String.Join( ",", workplaces.Select( n => n.ToString() ).ToArray() );
            }
            set
            {
                var old = workplaces;
                // Split value. and convert to int[] if the string is well formed
                if (RegexXmlIntegerValue.IsMatch(value))
                {
                    _oldWorkplacesStyle = true;
                    workplaces = new int[] { Convert.ToInt32(value), 0, 0, 0 };
                }
                else
                {
                    _oldWorkplacesStyle = false;

                    if (RegexXML4IntegerValues.IsMatch(value))
                    {
                        workplaces = value.Replace(" ", "").Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                    }
                    else
                    {
                        workplaces = new int[] { 0, 0, 0, 0 };
                    }
                }
                HandleSetterEvents( !old.SequenceEqual( workplaces ) );
            }
        }

        private string _workplaceDeviationString;
        //Workplace job distribution settings
        [XmlAttribute( "deviations" )]
        [DefaultValue( "0,0,0,0" )]
        public string workplaceDeviationString
        {
            get
            {
                return String.Join( ",", workplaceDeviation.Select( i => i.ToString() ).ToArray() );
            }
            set
            {
                var v = _workplaceDeviationString;
                _workplaceDeviationString = value;

                // Split values and convert to int[] if the string is well formed
                if ( RegexXML4IntegerValues.IsMatch( value ) )
                    this.workplaceDeviation = value.Replace( " ", "" ).Split( ',' ).Select( n => Convert.ToInt32( n ) ).ToArray();
                else
                    this.workplaceDeviation = new int[] { 0, 0, 0, 0 };


                _lastSetDeviations = workplaceDeviation.Select( n => n ).ToArray();

                HandleSetterEvents( v != value );
            }
        }

        private int[] _lastSetDeviations;

        // Flag wether to ignore the realistic population mod is running 
        // This should probably be "true" by default
        [XmlAttribute( "ignore-reality" )]
        [DefaultValue( false )]
        public bool RealityIgnored { get { return _RealityIgnored; } set { var v = _RealityIgnored; _RealityIgnored = value; HandleSetterEvents( v != value ); } }
        bool _RealityIgnored;

        [XmlIgnore]
        public bool useReality
        {
            get
            {
                // Check for Realistic Population Revisited or original WG Realistic Population, and obviously ignore-reality flag.
                return (!RealityIgnored && (Util.IsModEnabled(2025147082ul) || Util.IsModEnabled(426163185ul)));
            }
        }

        [XmlIgnore]
        public bool isDirty
        {
            get
            {
                return
                    _isDirty ||
                    !_workplaces.SequenceEqual( _lastSetWorkplaces ) ||
                    !_workplaceDeviations.SequenceEqual( _lastSetDeviations );
            }
        }
        bool _isDirty;

        public void clean()
        {
            var d = _isDirty;
            _lastSetWorkplaces = _workplaces.Select( n => n ).ToArray();
            _lastSetDeviations = _workplaceDeviations.Select( N => N ).ToArray();
            _isDirty = false;
            if ( d )
                parent.RaiseBuildingDirtynessChanged( this );
        }

        [XmlIgnore]
        public int maxLevel { get { return Util.MaxLevelOf( service, subService ); } }

        [XmlIgnore]
        public PloppableRICODefinition parent;

        [XmlIgnore]
        public FileInfo parentSourceFile { get { return parent != null ? parent.sourceFile : null; } }

        [XmlIgnore]
        public bool oldWorkplacesStyle { get { return _oldWorkplacesStyle; } }
        bool _oldWorkplacesStyle;

        [XmlIgnore]
        public int workplaceCount { get { return workplaces.Sum(); } }

        [XmlIgnore]
        public int[] workplaces
        {
            get
            {
                if ( service == "residential" )
                    return new int[] { 0, 0, 0, 0 };

                // Quickfix - replace old tag method of setting workplace levels to -1 (which was interfering with total job counts) with just toggling boolean flag.
                // TODO tidyup
                if ( oldWorkplacesStyle )//&& _workplaces[1] < 0 )
                {
                    var originalWorkplaces = _workplaces[0];
                    var d = Util.WorkplaceDistributionOf( service, subService, "Level" + level );
                    if ( d == null )
                        d = new int[] { 100, 100, 0, 0, 0 };

                    var a = WorkplaceAIHelper.distributeWorkplaceLevels(originalWorkplaces, d, new int [] { 0,0,0,0 });

                    for (var i = 0; i < 4; i++)
                    {
                        _workplaces[i] = a[i];
                    }

                    // Check and adjust for any rounding errors, assigning 'leftover' jobs to the lowest education level.
                    _workplaces[0] += (originalWorkplaces - _workplaces.Sum());

                    // Commenting out for now - possibily no longer needed.
                    // TODO: review and delete if appropriate.
                    // Debug.Log("RICO Revisited: " + originalWorkplaces + " old-format workplaces for building '" + name + "'; replacing with workplaces " + _workplaces[0] + " " + _workplaces[1] + " " + _workplaces[2] + " " + _workplaces[3] + ".");

                    // Reset flag; these workplaces are now updated.
                    _oldWorkplacesStyle = false;
                }
               return _workplaces;
            }
            set
            {
                var old = _workplaces.Select( n => n).ToArray();
                _workplaces = value;
                _lastSetWorkplaces = _workplaces.Select( n => n ).ToArray();
                HandleSetterEvents( !old.SequenceEqual( _workplaces ) );
            }
        }

        private int[] _lastSetWorkplaces;
        private int[] _workplaces;


        [XmlIgnore]
        public int[] workplaceDeviation
        {
            get { if ( this.service == "residential" ) return new int[] { 0, 0, 0, 0 }; return _workplaceDeviations; }
            set
            {
                if 
                (
                    ( _workplaceDeviations == null && value != null ) ||
                    ( _workplaceDeviations != null && value == null ) 
                )
                {
                    _workplaceDeviations = value;
                    HandleSetterEvents( true );
                }
                else if ( _workplaceDeviations == null && value == null )
                {
                    _workplaceDeviations = value;
                    HandleSetterEvents( false );
                }
                else
                {
                    var old = _workplaceDeviations.Select( n => n ).ToArray();
                    _workplaceDeviations = value;
                    HandleSetterEvents( !old.SequenceEqual( value ) );
                }
            }
        }
        private int[] _workplaceDeviations;

        [XmlIgnore]
        public StringBuilder fatalErrors
        {
            // Detect and report any fatal errors.
            get
            {
                var errors = new StringBuilder();

                // Name errors.  Can't do anything with an invalid name.
                if (!new Regex(String.Format(@"[^<>:/\\\|\?\*{0}]", "\"")).IsMatch(name) || name == "* unnamed")
                {
                    errors.AppendLine(String.Format("A building has {0} name.", name == "" || name == "* unnamed" ? "no" : "a funny"));
                }

                // Service errors.  Can't do anything with an invalid service.
                if (!new Regex(@"^(residential|commercial|office|industrial|extractor|none|dummy)$").IsMatch(service))
                {
                    errors.AppendLine("Building '" + name + "' has " + (service == "" ? "no " : "an invalid ") + "service.");
                }


                // Sub-service errors.  We can work with office or industrial, but not commercial or residential.
                if (!new Regex(@"^(high|low|generic|farming|oil|forest|ore|none|tourist|leisure|high tech|eco|high eco|low eco)$").IsMatch(subService))
                {
                    // Allow for invalid subservices for office and industrial buildings.
                    if (!(service == "office" || service == "industrial"))
                    {
                        errors.AppendLine("Building '" + name + "' has " + (service == "" ? "no " : "an invalid ") + "sub-service.");
                    }
                    else
                    {
                        // If office or industrial, at least reset subservice to something decent.
                        Debug.Log("RICO Revisited: Building '" + name + "' has " + (service == "" ? "no " : "an invalid ") + "sub-service.  Resetting to 'generic'.");
                        subService = "generic";
                    }
                }

                // Workplaces.  Need something to go with, here.
                if (!(RegexXML4IntegerValues.IsMatch(workplacesString) || RegexXmlIntegerValue.IsMatch(workplacesString)))
                {
                    errors.AppendLine("Building '" + name + "' has an invalid value for 'workplaces'. Must be either a positive integer number or a comma separated list of 4 positive integer numbers.");
                }

                // Deviations.  Also need something.
                if (!RegexXML4IntegerValues.IsMatch(workplaceDeviationString))
                {
                    errors.AppendLine("Building '" + name + "' has an invalid value for 'deviations'. Must be either a positive integer number or a comma separated list of 4 positive integer numbers.");
                }

                return errors;
            }
        }

        [XmlIgnore]
        public StringBuilder nonFatalErrors
        {
            // Errors that we can recover from, or at least work with.
            // Should only be used after fatalErrors so that we know we've got legitimate service and sub-service values to work with.
            get
            {
                var errors = new StringBuilder();


                if (!new Regex(@"^(comlow|comhigh|reslow|reshigh|office|industrial|oil|ore|farming|forest|tourist|leisure|organic|hightech|selfsufficient)$").IsMatch(uiCategory))
                {
                    // Invalid UI Category; calculate new one from scratch.
                    string newCategory = string.Empty;

                    switch(service)
                    {
                        case "residential":
                            switch (subService)
                            {
                                case "low":
                                    newCategory = "reslow";
                                    break;
                                case "high eco":
                                case "low eco":
                                    newCategory = "selfsufficient";
                                    break;
                                default:
                                    newCategory = "reshigh";
                                    break;
                            }
                            break;
                        case "industrial":
                            switch(subService)
                            {
                                case "farming":
                                case "forest":
                                case "oil":
                                case "ore":
                                    newCategory = subService;
                                    break;
                                default:
                                    newCategory = "industrial";
                                    break;
                            }
                            break;
                        case "commercial":
                            switch(subService)
                            {
                                case "low":
                                    newCategory = "comlow";
                                    break;
                                case "tourist":
                                case "leisure":
                                    newCategory = subService;
                                    break;
                                case "eco":
                                    newCategory = "organic";
                                    break;
                                default:
                                    newCategory = "comhigh";
                                    break;
                            }
                            break;
                        case "office":
                            if (subService == "high tech")
                                newCategory = "hightech";
                            else
                                newCategory = "generic";
                            break;
                    }

                    // If newCategory is still empty, we didn't work it out.
                    if (string.IsNullOrEmpty(newCategory))
                    {
                        errors.AppendLine("Building '" + name + "' has an invalid ui-category '" + uiCategory + "'.");
                    }
                    else
                    {
                        errors.AppendLine("Building '" + name + "' has an invalid ui-category '" + uiCategory + "'; reverting to '" + newCategory + "'.");
                        uiCategory = newCategory;
                    }
                }

                // Check home and workplace counts, as appropriate.
                if (service == "residential")
                {
                    if (homeCount == 0)
                    {
                        // If homeCount is zero, check to see if any workplace count has been entered instead (I'm looking at you, One Vanderbilt - New York).
                        if (_workplaces.Sum() > 0)
                        {
                            homeCount = _workplaces.Sum();
                            errors.AppendLine("Building '" + name + "' is 'residential' but has zero homes; using workplaces count of " + homeCount + " instead.");
                        }
                        else
                        {
                            errors.AppendLine("Building '" + name + "' is 'residential' but no homes are set.");
                        }
                    }
                }
                else
                {
                    if ((workplaceCount == 0) && service != "" && service != "none")
                    {
                        errors.AppendLine("Building '" + name + "' provides " + service + " jobs but no jobs are set.");
                    }
                }

                // Building level.  Basically check and clamp to 1 <= level <= maximum level (for this category and sub-category combination).
                int newLevel = Math.Min(Math.Max(level, 1), maxLevel);

                if (newLevel != level)
                {
                    if (newLevel == 1)
                    {
                        // Don't bother reporting errors for levels reset to 1, as those are generally for buildings that only have one level anwyay and it's just annoying users.
                        Debug.Log("RICO Revisited: building '" + name + "' has invalid level '" + level.ToString() + "'. Resetting to level '" + newLevel + "'.");
                    }
                    else
                    {
                        errors.AppendLine("Building '" + name + "' has invalid level '" + level.ToString() + "'. Resetting to level '" + newLevel + "'.");
                    }
                    level = newLevel;
                }

                return errors;
            }
        }

        // attributes routed trough crp parsing

        private CrpData _crpData;
        [XmlIgnore]
        public CrpData crpData
        {
            get { return _crpData; }
            set { _crpData = value; steamId = value.SteamId; }
        }
        [XmlIgnore]
        public object previewImage { get { return crpData != null ? crpData.PreviewImage : null; } }
        [XmlIgnore]
        public string tags { get { return crpData != null ? crpData.Tags : ""; } }
        [XmlIgnore]
        public string type { get { return crpData != null ? crpData.Type : ""; } }
        [XmlIgnore]
        public string authorID { get { return crpData != null ? crpData.AuthorID : ""; } }
        [XmlIgnore]
        public string buildingName { get { return crpData != null ? crpData.BuildingName : ""; } }

        // fetching stuff from steam
        [XmlIgnore]
        public ISteamDataProvider steamDataProvider;

        [XmlIgnore]
        public SteamData steamData;
        private int steamDataTries = 0;

        [XmlIgnore]
        public string authorName
        {
            get
            {
                if ( steamDataTries++ < 3 && steamDataProvider != null && steamData == null && steamId != null && steamId != "" )
                    steamData = steamDataProvider.getSteamData( steamId );

                if ( steamData != null )
                    return steamData.AuthorName;
                else
                    return "n/a";
            }
        }
    }

}