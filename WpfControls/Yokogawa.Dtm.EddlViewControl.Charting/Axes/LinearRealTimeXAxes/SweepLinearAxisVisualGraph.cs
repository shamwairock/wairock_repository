using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class SweepLinearAxisVisualGraph : AxisVisualGraph, IGridLineProvider
    {
        public ISweepLinearAxisVisualModel SweepLinearAxisModel { get; set; }

        public override void Render(AxisVisualContext dataContext)
        {
            if (SweepLinearAxisModel.ShowMajorTick == false && SweepLinearAxisModel.ShowMinorTick == false)
            {
                return;
            }

            axisVisualContext = dataContext as AxisVisualContext;
            if (axisVisualContext == null || axisVisualContext.Render == null)
            {
                return;
            }

            InitialzeTicks();
            CalculateTicks();
            DrawTicks();
        }

        private AxisVisualContext axisVisualContext = null;

        private DrawingContext RenderOpen()
        {
            return axisVisualContext.Render.AxisRenderOpen();
        }

        private LinearAxisGrapScale preAxis = new LinearAxisGrapScale();

        private LinearAxisGrapScale postAxis = new LinearAxisGrapScale();

        private LinearTickCalculator tickCalculator = new LinearTickCalculator();

        private MinorLinearTicksCalculator minorTickCalculator = new MinorLinearTicksCalculator();

        public SweepLinearXAxis SweepLinearXAxis { get; set; }

        private void CalculateTicks()
        {
            tickCalculator.Initialize(new Range<double>(preAxis.Minimum, preAxis.Maximum));
            do
            {
                tickCalculator.DescentTickCount();
                var preticks = tickCalculator.CreateTicks(new Range<double>(preAxis.Minimum, preAxis.Maximum));
                if (preticks == null || preticks.Length <= 1)
                {
                    break;
                }
            } while (true);

            TickFormattedText[] checkTickTexts;
            LinearTickPosition[] checkTickPoses;
            GridLinePosition[] checkGridLines;
            CreateTickTexts(out checkTickTexts, out checkTickPoses, out checkGridLines);
            if (checkTickTexts.Length > 0 && CheckLabelsArrangement(checkTickTexts) == TickCountChange.Decrease)
            {
                return;
            }

            var maxTickCount = GetMaxTickCount();
            do
            {
                tickCalculator.IncreaseTickCount();

                TickFormattedText[] tickTexts;
                LinearTickPosition[] tickPoses;
                GridLinePosition[] cgridLines;
                CreateTickTexts(out tickTexts, out tickPoses, out cgridLines);
                var result = CheckLabelsArrangement(tickTexts);
                if (result == TickCountChange.Decrease)
                {
                    tickCalculator.DescentTickCount();
                    break;
                }
                if (tickTexts.Length >= maxTickCount + 1)
                {
                    tickCalculator.DescentTickCount();
                    break;
                }
            } while (true);

            if (SweepLinearAxisModel.ShowMinorTick)
            {
                CreateMinorTicks();
            }
        }

        private void CreateTickTexts(out TickFormattedText[] texts, out LinearTickPosition[] ticks, out GridLinePosition[] lines)
        {
            var checkPreTicks = tickCalculator.CreateTicks(new Range<double>(preAxis.Minimum, preAxis.Maximum));
            var checkPostTicks = tickCalculator.CreateTicks(new Range<double>(postAxis.Minimum, postAxis.Maximum));

            var tickTexts = new List<TickFormattedText>();
            var tickPoses = new List<LinearTickPosition>();
            var gridLines = new List<GridLinePosition>();
            if (checkPreTicks != null && checkPreTicks.Length > 0)
            {
                checkPreTicks.ToList().ForEach((tick) =>
                {
                    if (SweepLinearXAxis.PostPartValid == false || SweepLinearXAxis.InPrePartRange(tick))
                    {
                        var text = new TickFormattedText()
                        {
                            Position = preAxis.GetPositionByValue(tick),
                            Text = GetTickLabelFormator().GetFormattedText(AxisModel.DataConverter != null ? AxisModel.DataConverter.ToDataValue(tick) : tick),
                        };
                        tickTexts.Add(text);
                        var t = new LinearTickPosition()
                        {
                            Value = tick,
                            Position = text.Position,
                            Length = SweepLinearAxisModel.MajorTickLength,
                        };
                        tickPoses.Add(t);
                        gridLines.Add(new GridLinePosition(text.Position, true));
                    }
                });
            }

            if (SweepLinearXAxis.PostPartValid  && checkPostTicks != null && checkPostTicks.Length > 0)
            {
                checkPostTicks.ToList().ForEach((tick) =>
                {
                    if (SweepLinearXAxis.InPostPartRange(tick))
                    {
                        var text = new TickFormattedText()
                        {
                            Position = postAxis.GetPositionByValue(tick),
                            Text = GetTickLabelFormator().GetFormattedText(AxisModel.DataConverter != null ? AxisModel.DataConverter.ToDataValue(tick) : tick),
                        };
                        tickTexts.Add(text);
                        var t = new LinearTickPosition()
                        {
                            Value = tick,
                            Position = text.Position,
                            Length = SweepLinearAxisModel.MajorTickLength,
                        };
                        tickPoses.Add(t);
                        gridLines.Add(new GridLinePosition(text.Position, true));
                    }
                });
            }

            texts = tickTexts.ToArray();
            ticks = tickPoses.ToArray();
            lines = gridLines.ToArray();
        }

        private void CreateMinorTicks()
        {
            minorTickCalculator.Initialize();

            Range<double> preAxisRange, postAxisRange;
            List<Range<double>> preRanges, postRanges;
            CreateCurrentMajorTick(out preAxisRange, out postAxisRange, out preRanges, out postRanges);

            var result = TickCountChange.OK;
            var prevResult = TickCountChange.OK;
            int prevCount = -1;
            do
            {
                var minorTicks = CreateCurrentMinorTicks(preAxisRange, postAxisRange, preRanges, postRanges);
                result = CheckMinorTicksArrangement(minorTicks.ToArray());
                if (prevResult == TickCountChange.Decrease && result == TickCountChange.Increase)
                {
                    result = TickCountChange.OK;
                }
                if (result == TickCountChange.Decrease)
                {
                    if (minorTicks.Length <= 0)
                    {
                        break;
                    }
                    minorTickCalculator.DecreaseTickCount();
                }
                else if (result == TickCountChange.Increase)
                {
                    if (prevCount == minorTicks.Length)
                    {
                        break;
                    }
                    minorTickCalculator.IncreaseTickCount();
                }
                prevResult = result;
                prevCount = minorTicks.Length;
            } while (result != TickCountChange.OK);
        }

        private LinearTickPosition[] CreateCurrentMinorTicks(Range<double> preAxisRange, Range<double> postAxisRange, List<Range<double>> preRanges, List<Range<double>> postRanges)
        {
            var preMinorTicks = minorTickCalculator.CreateTicks(preRanges.ToArray(), preAxisRange);
            var postMinorTicks = minorTickCalculator.CreateTicks(postRanges.ToArray(), postAxisRange);
            var minorTicks = new List<LinearTickPosition>();
            if (preMinorTicks != null && preMinorTicks.Length > 0)
            {
                preMinorTicks.ToList().ForEach((tick) =>
                {
                    if (SweepLinearXAxis.PostPartValid || SweepLinearXAxis.InPrePartRange(tick))
                    {
                        var t = new LinearTickPosition()
                        {
                            Value = tick,
                            Position = preAxis.GetPositionByValue(tick),
                            Length = SweepLinearAxisModel.MinorTickLength,
                        };
                        minorTicks.Add(t);
                    }
                });
            }
            if (SweepLinearXAxis.PostPartValid && postMinorTicks != null && postMinorTicks.Length > 0)
            {
                postMinorTicks.ToList().ForEach((tick) =>
                {
                    if (SweepLinearXAxis.InPostPartRange(tick))
                    {
                        var t = new LinearTickPosition()
                        {
                            Value = tick,
                            Position = postAxis.GetPositionByValue(tick),
                            Length = SweepLinearAxisModel.MinorTickLength,
                        };
                        minorTicks.Add(t);
                    }
                });
            }

            return minorTicks.ToArray();
        }

        private void CreateCurrentMajorTick(out Range<double> preAxisRange, out Range<double> postAxisRange, out List<Range<double>> preRanges, out List<Range<double>> postRanges)
        {
            preAxisRange = new Range<double>(preAxis.Minimum, preAxis.Maximum);
            postAxisRange = new Range<double>(postAxis.Minimum, postAxis.Maximum);

            var allPreTicks = tickCalculator.CreateTicks(preAxisRange);
            preRanges = new List<Range<double>>();
            if (allPreTicks.Length > 0)
            {
                for (int i = 0; i < (allPreTicks.Length - 1); i++)
                {
                    preRanges.Add(new Range<double>(allPreTicks[i], allPreTicks[i + 1]));
                }
            }

            var allPostTicks = tickCalculator.CreateTicks(postAxisRange);
            postRanges = new List<Range<double>>();
            if (allPostTicks.Length > 0)
            {
                for (int i = 0; i < (allPostTicks.Length - 1); i++)
                {
                    postRanges.Add(new Range<double>(allPostTicks[i], allPostTicks[i + 1]));
                }
            }
        }

        private int GetMaxTickCount()
        {
            var actualExtent = Math.Abs(preAxis.StopPosistion - preAxis.StartPosistion);
            int maxTickcount = (int)(actualExtent / (AxisModel.FontSize * 1.01));
            if (maxTickcount <= 1)
            {
                maxTickcount = 2;
            }
            return maxTickcount;
        }

        private class TickFormattedText
        {
            public double Position { get; set; }
            public FormattedText Text { get; set; }
        }

        private LinearTickLabelFormator tickLabelFormator;

        private LinearTickLabelFormator GetTickLabelFormator()
        {
            if (tickLabelFormator == null)
            {
                tickLabelFormator = new LinearTickLabelFormator(new Typeface(AxisModel.FontFamily, AxisModel.FontStyle, AxisModel.FontWeight, AxisModel.FontStretch), System.Windows.FlowDirection.LeftToRight, AxisModel.FontSize, AxisModel.LabelBrush)
                {
                    CustomFormatter = AxisModel.CustomFormatter,
                };
            }
            return tickLabelFormator;
        }

        private TickFormattedText[] CreateTickLabels(LinearAxisGrapScale axis, double[] ticks)
        {
            var ret = new List<TickFormattedText>();

            GetTickLabelFormator().InitTicks(ticks);

            if (ticks != null && ticks.Length > 0)
            {
                foreach (var tick in ticks)
                {
                    var text = new TickFormattedText()
                    {
                        Position = axis.GetPositionByValue(tick),
                        Text = GetTickLabelFormator().GetFormattedText(AxisModel.DataConverter != null ? AxisModel.DataConverter.ToDataValue(tick) : tick),
                    };
                    ret.Add(text);
                }
            }
            return ret.ToArray();
        }

        private double GetLabelCheckSize(FormattedText text)
        {
            return AxisModel.Orientation == AxisOrientation.Horizontal ? text.Width : text.Height;
        }

        private double GetIncreaseRatio()
        {
            return AxisModel.Orientation == AxisOrientation.Horizontal ? 2.0 : 3;
        }

        private double GetDecreaseRatio()
        {
            return AxisModel.Orientation == AxisOrientation.Horizontal ? 1.5 : 2;
        }

        private TickCountChange CheckLabelsArrangement(TickFormattedText[] tickTexts)
        {
            var ret = tickTexts.Length != 0 ? TickCountChange.OK : TickCountChange.Decrease;

            int increaseCount = 0;
            for (int i = 0; i < tickTexts.Length - 1; i++)
            {
                if (Math.Abs(tickTexts[i + 1].Position - tickTexts[i].Position) < (GetLabelCheckSize(tickTexts[i + 1].Text) + GetLabelCheckSize(tickTexts[i].Text)) / 2.0 * GetDecreaseRatio())
                {
                    ret = TickCountChange.Decrease;
                    break;
                }
                if (Math.Abs(tickTexts[i + 1].Position - tickTexts[i].Position) > (GetLabelCheckSize(tickTexts[i + 1].Text) + GetLabelCheckSize(tickTexts[i].Text)) / 2.0 * GetIncreaseRatio())
                {
                    increaseCount++;
                }
            }

            if (ret != TickCountChange.Decrease)
            {
                if (increaseCount > tickTexts.Length / 2)
                {
                    ret = TickCountChange.Increase;
                }
                else if (tickTexts.Length == 2 && increaseCount > 0)
                {
                    ret = TickCountChange.Increase;
                }
                else if (tickTexts.Length == 1)
                {
                    ret = TickCountChange.Increase;
                }
            }
            return ret;
        }

        private TickCountChange CheckMinorTicksArrangement(LinearTickPosition[] minorTicks)
        {
            var result = TickCountChange.OK;
            if (minorTicks != null && minorTicks.Length > 1)
            {
                int count = minorTicks.Length;
                for (int i = 1; i < count; i++)
                {
                    var preTick = minorTicks[i - 1].Position;
                    var currentTick = minorTicks[i].Position;
                    if (Math.Abs(currentTick - preTick) < 5)
                    {
                        result = TickCountChange.Decrease;
                        break;
                    }
                    else if (Math.Abs(currentTick - preTick) > 2)
                    {
                        result = TickCountChange.Increase;
                        break;
                    }
                }
            }
            return result;
        }

        private void DrawTicks()
        {
            using (var dc = RenderOpen())
            {
                if (AxisModel.Orientation == AxisOrientation.Horizontal)
                {
                    if ((AxisModel.ContentLayout & AxisContentLayout.AtBelow) != 0)
                    {
                        DrawTicksAtBelow(dc);
                    }
                    else if ((AxisModel.ContentLayout & AxisContentLayout.AtAbove) != 0)
                    {
                        DrawTicksAtAbove(dc);
                    }
                }
                else if (AxisModel.Orientation == AxisOrientation.Vertical)
                {
                    if ((AxisModel.ContentLayout & AxisContentLayout.AtLeft) != 0)
                    {
                        DrawTicksAtLeft(dc);
                    }
                    else if ((AxisModel.ContentLayout & AxisContentLayout.AtRight) != 0)
                    {
                        DrawTicksAtRight(dc);
                    }
                }
            }
        }

        private void DrawTicksAtRight(DrawingContext dc)
        {
            PrepareTicks();

            double lineStartPos = preAxis.StopPosistion;
            double lineStopPos = preAxis.StartPosistion;

            double lineTotal = lineStopPos - lineStartPos;
            double longTickLength = SweepLinearAxisModel.MajorTickLength;
            double shortTickLength = SweepLinearAxisModel.MinorTickLength;
            bool centerTicks = (AxisModel.ContentLayout & AxisContentLayout.TicksCentered) != 0;

            double titleCenter = (lineStartPos + lineStopPos) / 2.0;

            double leftPosition = 0.0;

            double baseLine;
            if (centerTicks)
            {
                baseLine = leftPosition + longTickLength / 2;
            }
            else
            {
                baseLine = leftPosition + AxisModel.TickBaseLinePen.Thickness / 2.0;
            }

            if (mayorTickInfos != null && mayorTickInfos.Length > 0)
            {
                var tickOffset = AxisModel.TickBaseLinePen.Thickness / 2.0;
                foreach (var tick in mayorTickInfos)
                {
                    double mayorTop = tick.Position;
                    if (centerTicks)
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(baseLine - longTickLength / 2.0, mayorTop), new Point(baseLine + longTickLength / 2.0, mayorTop));
                    }
                    else
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(baseLine + tickOffset, mayorTop), new Point(baseLine + tickOffset + longTickLength, mayorTop));
                    }
                }
            }

            if (minorTickInfos != null && minorTickInfos.Length > 0)
            {
                var tickOffset = AxisModel.TickBaseLinePen.Thickness / 2.0;
                foreach (var tick in minorTickInfos)
                {
                    double minTop = tick.Position;
                    if (centerTicks)
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(baseLine - shortTickLength / 2.0, minTop), new Point(baseLine + shortTickLength / 2.0, minTop));
                    }
                    else
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(baseLine + tickOffset, minTop), new Point(baseLine + tickOffset + shortTickLength, minTop));
                    }
                }
            }

            dc.DrawLine(AxisModel.TickBaseLinePen, new Point(baseLine, lineStopPos), new Point(baseLine, lineStartPos));
            if (centerTicks)
            {
                leftPosition = baseLine + longTickLength / 2.0;
            }
            else
            {
                leftPosition = baseLine + AxisModel.TickBaseLinePen.Thickness / 2.0 + longTickLength;
            }

            if (mayorTickLabels != null && mayorTickLabels.Length > 0)
            {
                double labelPos = leftPosition + AxisModel.LabelMargin;
                double labelTop = 0.0;
                foreach (var tick in mayorTickLabels)
                {
                    labelTop = tick.Position - tick.Text.Height / 2;

                    dc.DrawText(tick.Text, new Point(labelPos, labelTop));

                    if (leftPosition < labelPos + tick.Text.Width)
                    {
                        leftPosition = labelPos + tick.Text.Width;
                    }
                }
            }

            var titleText = GetFormatedTitle();
            if (AxisModel.ShowTitle && titleText != null)
            {
                var titleTop = titleCenter + titleText.Width / 2.0;
                var titlePos = leftPosition;
                DrawRotateText(dc, titleText, titlePos, titleTop, -90);
                leftPosition = titlePos + titleText.Height + AxisModel.TitleMargin;
            }

            renderLeft = 0.0;
            renderRight = leftPosition;
            renderTop = 0.0;
            renderBottom = AxisModel.PlotLength;
        }

        private void DrawTicksAtLeft(DrawingContext dc)
        {
            PrepareTicks();

            double lineStartPos = preAxis.StopPosistion;
            double lineStopPos = preAxis.StartPosistion;

            double lineTotal = lineStopPos - lineStartPos;
            double longTickLength = SweepLinearAxisModel.MajorTickLength;
            double shortTickLength = SweepLinearAxisModel.MinorTickLength;
            bool centerTicks = (AxisModel.ContentLayout & AxisContentLayout.TicksCentered) != 0;

            double titleCenter = (lineStartPos + lineStopPos) / 2.0;

            double leftPosition = AxisModel.TitleMargin;

            var titleText = GetFormatedTitle();
            if (AxisModel.ShowTitle && titleText != null)
            {
                var titleTop = titleCenter + titleText.Width / 2.0;
                var titlePos = leftPosition;
                DrawRotateText(dc, titleText, titlePos, titleTop, -90);
                leftPosition = titlePos + titleText.Height;
            }

            if (mayorTickLabels != null && mayorTickLabels.Length > 0)
            {
                double labelPos = leftPosition + AxisModel.LabelMargin;
                double labelTop = 0.0;
                foreach (var tick in mayorTickLabels)
                {
                    labelTop = tick.Position - tick.Text.Height / 2;

                    dc.DrawText(tick.Text, new Point(labelPos, labelTop));

                    if (leftPosition < labelPos + tick.Text.Width)
                    {
                        leftPosition = labelPos + tick.Text.Width;
                    }
                }
            }

            double baseLine;
            if (centerTicks)
            {
                baseLine = leftPosition + longTickLength / 2;
            }
            else
            {
                baseLine = leftPosition + longTickLength + AxisModel.TickBaseLinePen.Thickness / 2.0;
            }

            if (mayorTickInfos != null && mayorTickInfos.Length > 0)
            {
                var tickOffset = AxisModel.TickBaseLinePen.Thickness / 2.0;
                foreach (var tick in mayorTickInfos)
                {
                    double mayorTop = tick.Position;
                    if (centerTicks)
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(baseLine - longTickLength / 2.0, mayorTop), new Point(baseLine + longTickLength / 2.0, mayorTop));
                    }
                    else
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(baseLine - longTickLength - tickOffset, mayorTop), new Point(baseLine - tickOffset, mayorTop));
                    }
                }
            }

            if (minorTickInfos != null && minorTickInfos.Length > 0)
            {
                var tickOffset = AxisModel.TickBaseLinePen.Thickness / 2.0;
                foreach (var tick in minorTickInfos)
                {
                    double minTop = tick.Position;
                    if (centerTicks)
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(baseLine - shortTickLength / 2.0, minTop), new Point(baseLine + shortTickLength / 2.0, minTop));
                    }
                    else
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(baseLine - shortTickLength - tickOffset, minTop), new Point(baseLine - tickOffset, minTop));
                    }
                }
            }

            dc.DrawLine(AxisModel.TickBaseLinePen, new Point(baseLine, lineStopPos), new Point(baseLine, lineStartPos));
            if (centerTicks)
            {
                leftPosition = baseLine + longTickLength / 2.0;
            }
            else
            {
                leftPosition = baseLine + AxisModel.TickPen.Thickness / 2.0;
            }

            renderLeft = 0.0;
            renderRight = leftPosition;
            renderTop = 0.0;
            renderBottom = AxisModel.PlotLength;
        }

        private void DrawTicksAtAbove(DrawingContext dc)
        {
            PrepareTicks();

            double lineStartPos = preAxis.StopPosistion;
            double lineStopPos = preAxis.StartPosistion;

            double lineTotal = lineStopPos - lineStartPos;
            double longTickLength = SweepLinearAxisModel.MajorTickLength;
            double shortTickLength = SweepLinearAxisModel.MinorTickLength;
            bool centerTicks = (AxisModel.ContentLayout & AxisContentLayout.TicksCentered) != 0;

            double topPosition = AxisModel.TitleMargin;

            double titleCenter = (lineStartPos + lineStopPos) / 2.0;

            var titleText = GetFormatedTitle();
            if (AxisModel.ShowTitle && titleText != null)
            {
                var titlePos = titleCenter - titleText.Width / 2.0;
                var titleTop = topPosition;
                dc.DrawText(titleText, new Point(titlePos, titleTop));
                topPosition = titleTop + titleText.Height;
            }

            if (mayorTickLabels != null && mayorTickLabels.Length > 0)
            {
                double labelTop = topPosition + AxisModel.LabelMargin;
                double labelPos = 0;
                foreach (var tick in mayorTickLabels)
                {
                    labelPos = tick.Position - tick.Text.Width / 2;
                    dc.DrawText(tick.Text, new Point(labelPos, labelTop));
                    if (topPosition < labelTop + tick.Text.Height)
                    {
                        topPosition = labelTop + tick.Text.Height;
                    }
                }
            }

            double baseLine;
            if (centerTicks)
            {
                baseLine = topPosition + longTickLength / 2;
            }
            else
            {
                baseLine = topPosition + longTickLength + AxisModel.TickBaseLinePen.Thickness / 2.0;
            }

            if (mayorTickInfos != null && mayorTickInfos.Length > 0)
            {
                var tickoffset = AxisModel.TickBaseLinePen.Thickness / 2.0;
                foreach (var tick in mayorTickInfos)
                {
                    double tickPos = tick.Position;
                    if (centerTicks)
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(tickPos, baseLine - longTickLength / 2), new Point(tickPos, baseLine + longTickLength / 2));
                    }
                    else
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(tickPos, baseLine - tickoffset), new Point(tickPos, baseLine - longTickLength - tickoffset));
                    }
                }
            }

            if (minorTickInfos != null && minorTickInfos.Length > 0)
            {
                var tickoffset = AxisModel.TickBaseLinePen.Thickness / 2.0;
                foreach (var tick in minorTickInfos)
                {
                    double tickPos = tick.Position;
                    if (centerTicks)
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(tickPos, baseLine - shortTickLength / 2), new Point(tickPos, baseLine + shortTickLength));
                    }
                    else
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(tickPos, baseLine - tickoffset), new Point(tickPos, baseLine - shortTickLength - tickoffset));
                    }
                }
            }

            dc.DrawLine(AxisModel.TickBaseLinePen, new Point(lineStopPos, baseLine), new Point(Math.Abs(lineStartPos), baseLine));
            if (centerTicks)
            {
                topPosition = baseLine + longTickLength / 2.0;
            }
            else
            {
                topPosition = baseLine + AxisModel.TickBaseLinePen.Thickness / 2.0;
            }

            renderLeft = 0.0;
            renderRight = AxisModel.PlotLength;
            renderTop = 0.0;
            renderBottom = topPosition;

        }

        private void DrawTicksAtBelow(DrawingContext dc)
        {
            PrepareTicks();

            double lineStartPos = preAxis.StartPosistion;
            double lineStopPos = preAxis.StopPosistion;

            double lineTotal = lineStopPos - lineStartPos;

            double longTickLength = SweepLinearAxisModel.MajorTickLength;
            double shortTickLength = SweepLinearAxisModel.MinorTickLength;
            bool centerTicks = (AxisModel.ContentLayout & AxisContentLayout.TicksCentered) != 0;

            double titleCenter = (lineStartPos + lineStopPos) / 2.0;

            double topPosition = 0.0;

            double baseLine;
            if (centerTicks)
            {
                baseLine = topPosition + longTickLength / 2;
            }
            else
            {
                baseLine = topPosition + AxisModel.TickBaseLinePen.Thickness / 2.0;
            }
            if (mayorTickInfos != null && mayorTickInfos.Length > 0)
            {
                var tickOffset = AxisModel.TickBaseLinePen.Thickness / 2.0;
                foreach (var tick in mayorTickInfos)
                {
                    double tickPos = tick.Position;
                    if (centerTicks)
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(tickPos, baseLine - longTickLength / 2.0), new Point(tickPos, baseLine + longTickLength / 2.0));
                    }
                    else
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(tickPos, baseLine + tickOffset), new Point(tickPos, baseLine + longTickLength + tickOffset));
                    }
                }
            }

            if (minorTickInfos != null && minorTickInfos.Length > 0)
            {
                var tickOffset = AxisModel.TickBaseLinePen.Thickness / 2.0;
                foreach (var tick in minorTickInfos)
                {
                    double tickPos = tick.Position;
                    if (centerTicks)
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(tickPos, baseLine - shortTickLength / 2.0), new Point(tickPos, baseLine + shortTickLength / 2.0));
                    }
                    else
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(tickPos, baseLine + tickOffset), new Point(tickPos, baseLine + tickOffset + shortTickLength));
                    }
                }
            }

            dc.DrawLine(AxisModel.TickBaseLinePen, new Point(lineStopPos, baseLine), new Point(lineStartPos, baseLine));
            if (centerTicks)
            {
                topPosition = baseLine + longTickLength / 2.0;
            }
            else
            {
                topPosition = baseLine + AxisModel.TickBaseLinePen.Thickness / 2.0 + longTickLength;
            }

            if (mayorTickLabels != null && mayorTickLabels.Length > 0)
            {
                double labelTop = topPosition + AxisModel.LabelMargin;
                double labelPos = 0;
                foreach (var tick in mayorTickLabels)
                {
                    labelPos = tick.Position - tick.Text.Width / 2;
                    dc.DrawText(tick.Text, new Point(labelPos, labelTop));
                    if (topPosition < labelTop + tick.Text.Height)
                    {
                        topPosition = labelTop + tick.Text.Height;
                    }
                }
            }

            var titleText = GetFormatedTitle();
            if (AxisModel.ShowTitle && titleText != null)
            {
                var titlePos = titleCenter - titleText.Width / 2.0;
                dc.DrawText(titleText, new Point(titlePos, topPosition));
                topPosition = topPosition + titleText.Height + AxisModel.TitleMargin;
            }

            renderLeft = 0.0;
            renderRight = AxisModel.PlotLength;
            renderTop = 0.0;
            renderBottom = topPosition;
        }

        private void DrawRotateText(DrawingContext dc, FormattedText label, double x, double y, double angle)
        {
            RotateTransform RT = new RotateTransform
            {
                Angle = angle,
                CenterX = x,
                CenterY = y
            };
            dc.PushTransform(RT);
            dc.DrawText(label, new Point(x, y));
            dc.Pop();
        }

        private FormattedText GetFormatedTitle()
        {
            if (!string.IsNullOrWhiteSpace(AxisModel.Title))
            {
                var titleformator = GetTitleLabelFormator();
                titleformator.Title = AxisModel.Title;
                var titleText = titleformator.GetFormattedText();
                return titleText;
            }
            else
            {
                return null;
            }
        }

        private TitleLabelFormator titleLabelFormator;

        private TitleLabelFormator GetTitleLabelFormator()
        {
            if (titleLabelFormator == null)
            {
                titleLabelFormator = new TitleLabelFormator(new Typeface(AxisModel.FontFamily, AxisModel.FontStyle, AxisModel.FontWeight, AxisModel.FontStretch), System.Windows.FlowDirection.LeftToRight, AxisModel.TitleFontSize, AxisModel.TitleBrush)
                {
                    CustomFormatter = AxisModel.TitleCustomFormatter,
                };
            }
            return titleLabelFormator;
        }

        public GridLinePosition[] GetGridLines()
        {
            return gridLines;
        }

        private LinearTickPosition[] mayorTickInfos;
        private TickFormattedText[] mayorTickLabels;
        private GridLinePosition[] gridLines;
        private LinearTickPosition[] minorTickInfos;

        private void InitialzeTicks()
        {
            mayorTickInfos = null;
            mayorTickLabels = null;

            minorTickInfos = null;

            tickLabelFormator = null;
            titleLabelFormator = null;

            UpdateAxisScale();
        }

        private void UpdateAxisScale()
        {
            if (AxisModel.Orientation == AxisOrientation.Horizontal)
            {
                preAxis.StartPosistion = AxisModel.PlotStartPos;
                preAxis.StopPosistion = AxisModel.PlotStopPos;

                postAxis.StartPosistion = AxisModel.PlotStartPos;
                postAxis.StopPosistion = AxisModel.PlotStopPos;
            }
            else
            {
                preAxis.StartPosistion = AxisModel.PlotStartPos;
                preAxis.StopPosistion = AxisModel.PlotStopPos;

                postAxis.StartPosistion = AxisModel.PlotStartPos;
                postAxis.StopPosistion = AxisModel.PlotStopPos;
            }
            if (SweepLinearAxisModel.PreMinimum.HasValue && SweepLinearAxisModel.PreMaximum.HasValue)
            {
                preAxis.Minimum = SweepLinearAxisModel.PreMinimum.Value;
                preAxis.Maximum = SweepLinearAxisModel.PreMaximum.Value;
            }
            if (SweepLinearAxisModel.PostMinimum.HasValue && SweepLinearAxisModel.PostMaximum.HasValue)
            {
                postAxis.Minimum = SweepLinearAxisModel.PostMinimum.Value;
                postAxis.Maximum = SweepLinearAxisModel.PostMaximum.Value;
            }
            preAxis.Update();
            postAxis.Update();
        }

        private void PrepareTicks()
        {
            if (SweepLinearAxisModel.ShowMajorTick)
            {
                CreateTickTexts(out mayorTickLabels, out mayorTickInfos, out gridLines);
            }

            if (SweepLinearAxisModel.ShowMinorTick)
            {
                Range<double> preAxisRange, postAxisRange;
                List<Range<double>> preRanges, postRanges;
                CreateCurrentMajorTick(out preAxisRange, out postAxisRange, out preRanges, out postRanges);
                minorTickInfos = CreateCurrentMinorTicks(preAxisRange, postAxisRange, preRanges, postRanges);

            }
        }

        protected double renderLeft = 0;
        public double RenderLeft { get { return renderLeft; } }

        protected double renderRight = 0;
        public double RenderRight { get { return renderRight; } }

        protected double renderTop = 0;
        public double RenderTop { get { return renderTop; } }

        protected double renderBottom = 0;
        public double RenderBottom { get { return renderBottom; } }

        public override Rect GetRenderBound()
        {
            return new Rect(new Point(RenderLeft, RenderTop), new Point(RenderRight, RenderBottom));
        }
    }
}
