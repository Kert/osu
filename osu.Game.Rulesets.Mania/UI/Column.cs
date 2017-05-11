﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE


using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Colour;
using osu.Framework.Input;
using osu.Game.Graphics;
using osu.Game.Rulesets.Mania.Timing;
using System.Collections.Generic;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Primitives;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Mania.Objects;
using osu.Game.Rulesets.Mania.Judgements;

namespace osu.Game.Rulesets.Mania.UI
{
    public class Column : Container, IHasAccentColour
    {
        private const float key_icon_size = 10;
        private const float key_icon_corner_radius = 3;
        private const float key_icon_border_radius = 2;

        private const float hit_target_height = 10;
        private const float hit_target_bar_height = 2;

        private const float column_width = 45;
        private const float special_column_width = 70;

        public Key Key;

        private readonly Box background;
        private readonly Container hitTargetBar;
        private readonly Container keyIcon;

        public readonly TimingSectionContainer TimingSectionContainer;

        public Column(IEnumerable<TimingSection> timingSections)
        {
            RelativeSizeAxes = Axes.Y;
            Width = column_width;

            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    Name = "Foreground",
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0.2f
                },
                new Container
                {
                    Name = "Key",
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    RelativeSizeAxes = Axes.X,
                    Height = ManiaPlayfield.HIT_TARGET_POSITION,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Name = "Key gradient",
                            RelativeSizeAxes = Axes.Both,
                            ColourInfo = ColourInfo.GradientVertical(Color4.Black, Color4.Black.Opacity(0)),
                            Alpha = 0.5f
                        },
                        keyIcon = new Container
                        {
                            Name = "Key icon",
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(key_icon_size),
                            Masking = true,
                            CornerRadius = key_icon_corner_radius,
                            BorderThickness = 2,
                            BorderColour = Color4.White, // Not true
                            Children = new[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Alpha = 0,
                                    AlwaysPresent = true
                                }
                            }
                        }
                    }
                },
                TimingSectionContainer = new TimingSectionContainer(timingSections)
                {
                    Name = "Hit objects",
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Y = -ManiaPlayfield.HIT_TARGET_POSITION
                },
                new Container
                {
                    Name = "Hit target",
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    RelativeSizeAxes = Axes.X,
                    Height = hit_target_height,
                    Y = -ManiaPlayfield.HIT_TARGET_POSITION,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Name = "Background",
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Black
                        },
                        hitTargetBar = new Container
                        {
                            Name = "Bar",
                            Anchor = Anchor.BottomCentre,
                            Origin = Anchor.BottomCentre,
                            RelativeSizeAxes = Axes.X,
                            Height = hit_target_bar_height,
                            Masking = true,
                            Children = new[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            }
                        }
                    }
                }
            };
        }

        private bool isSpecial;
        public bool IsSpecial
        {
            get { return isSpecial; }
            set
            {
                if (isSpecial == value)
                    return;
                isSpecial = value;

                Width = isSpecial ? special_column_width : column_width;
            }
        }

        private Color4 accentColour;
        public Color4 AccentColour
        {
            get { return accentColour; }
            set
            {
                if (accentColour == value)
                    return;
                accentColour = value;

                background.Colour = accentColour;

                hitTargetBar.EdgeEffect = new EdgeEffect
                {
                    Type = EdgeEffectType.Glow,
                    Radius = 5,
                    Colour = accentColour.Opacity(0.5f),
                };

                keyIcon.EdgeEffect = new EdgeEffect
                {
                    Type = EdgeEffectType.Glow,
                    Radius = 5,
                    Colour = accentColour.Opacity(0.5f),
                };
            }
        }

        public void Add(DrawableHitObject<ManiaHitObject, ManiaJudgement> hitObject)
        {
            TimingSectionContainer.Add(hitObject);
        }

        protected override bool OnKeyDown(InputState state, KeyDownEventArgs args)
        {
            if (args.Repeat)
                return false;

            if (args.Key == Key)
            {
                background.FadeTo(background.Alpha + 0.2f, 50, EasingTypes.OutQuint);
                keyIcon.ScaleTo(1.4f, 50, EasingTypes.OutQuint);
            }

            return false;
        }

        protected override bool OnKeyUp(InputState state, KeyUpEventArgs args)
        {
            if (args.Key == Key)
            {
                background.FadeTo(0.2f, 800, EasingTypes.OutQuart);
                keyIcon.ScaleTo(1f, 400, EasingTypes.OutQuart);
            }

            return false;
        }
    }
}
