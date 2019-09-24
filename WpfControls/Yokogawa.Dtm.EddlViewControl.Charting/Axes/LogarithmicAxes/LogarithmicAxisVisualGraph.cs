using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Yokogawa.Dtm.EddlViewControl.Charting
{
    public class LogarithmicAxisVisualGraph : AxisVisualGraph, IGridLineProvider
    {
        public ILogarithmicAxisVisualModel LogarithmicAxixModel { get; set; }

        public override void Render(AxisVisualContext context)
        {
            if (LogarithmicAxixModel.ShowMajorTick == false &&   LogarithmicAxixModel.ShowMinorTick == false)
            {
                return;
            }

            axisVisualContext = context as AxisVisualContext;
            if (axisVisualContext == null || axisVisualContext.Render == null)
            {
                return;
            }

            InitialzeTicks();
            CalculateLogarithmicTicks();
            DrawTicks();
        }

        private AxisVisualContext axisVisualContext = null;
        private DrawingContext RenderOpen()
        {
            return axisVisualContext.Render.AxisRenderOpen();
        }

        #region Logarithmic

        private LogarithmicGraphScale axis = new LogarithmicGraphScale();

        private LogarithmicTickLabelFormator tickLabelFormator;

        private LogarithmicTickCreator tickCreator = new LogarithmicTickCreator();

        private LogarithmicTickLabelFormator GetLogarithmicTickLabelFormator()
        {
            if (tickLabelFormator == null)
            {
                tickLabelFormator = new LogarithmicTickLabelFormator(new Typeface(AxisModel.FontFamily, AxisModel.FontStyle, AxisModel.FontWeight, AxisModel.FontStretch), System.Windows.FlowDirection.LeftToRight, AxisModel.FontSize, AxisModel.LabelBrush)
                {
                    CustomFormatter = AxisModel.CustomFormatter,
                };

            }
            return tickLabelFormator;
        }

        private void CalculateLogarithmicTicks()
        {
            tickCreator.SetRange(new Range<double>(axis.Minimum, axis.Maximum));

            //var maxTickCount = GetMaxTickCount();
            //bool hasDescented = false;
            int preTickCount = -1;
            do
            {
                tickCreator.IncreaseTickCount();
                var ticks = tickCreator.CreateTicks();
                if (ticks == null)
                {
                    break;
                }
                var labeledTicks = ticks.Count((t) => t.LabelVisible);
                if (preTickCount == labeledTicks)
                {
                    break;
                }
                //if (labeledTicks > maxTickCount)
                //{
                //    tickCreator.DescentTickCount();
                //    hasDescented = true;
                //    break;
                //}
                preTickCount = labeledTicks;
            } while (true);

            //if (hasDescented)
            //{
            //    preTickCount = -1;
            //    do
            //    {
            //        var ticks = tickCreator.CreateTicks();
            //        var labeledTicks = ticks.Count((t) => t.LabelVisible);
            //        if (labeledTicks <= maxTickCount)
            //        {
            //            break;
            //        }
            //        if (preTickCount == labeledTicks)
            //        {
            //            break;
            //        }
            //        tickCreator.DescentTickCount();
            //        preTickCount = labeledTicks;
            //    } while (true);
            //}

            var checkticks = tickCreator.CreateTicks();
            if (checkticks != null && checkticks.Length > 1 && CheckLabelsArrangement(checkticks) == TickCountChange.Increase)
            {
                return;
            }

            preTickCount = -1;
            do
            {
                tickCreator.DescentTickCount();
                var ticks = tickCreator.CreateTicks();
                var labeledTicks = ticks.Count((t) => t.LabelVisible);
                var result = CheckLabelsArrangement(ticks);
                if (result == TickCountChange.Increase || result == TickCountChange.OK)
                {
                    //if (preTickCount != -1)
                    //{
                    //    tickCreator.IncreaseTickCount();
                    //}
                    break;
                }
                if (preTickCount == labeledTicks)
                {
                    break;
                }
                preTickCount = labeledTicks;

            } while (true);
        }

        private TickFormattedText[] CreateLogarithmicTickLabels(LogarithmicTick[] ticks)
        {
            var ret = new List<TickFormattedText>();
            if (ticks != null && ticks.Length > 0)
            {
                foreach (var tick in ticks)
                {
                    if (tick.LabelVisible)
                    {
                        var text = new TickFormattedText()
                        {
                            Position = axis.GetPositionByValue(tick.Value.Value),
                            Text = GetLogarithmicTickLabelFormator().GetFormattedText((AxisModel.DataConverter != null ? AxisModel.DataConverter.ToDataValue(tick.Value.Value) : tick.Value.Value)),
                        };

                        ret.Add(text);
                    }
                }
            }
            return ret.ToArray();
        }

        private TickCountChange CheckLabelsArrangement(LogarithmicTick[] ticks)
        {
            return CheckLabelsArrangement(CreateLogarithmicTickLabels(ticks));
        }

        public GridLinePosition[] GetGridLines()
        {
            return gridLines;
        }

        //private LogarithmicTick[] logTicks;
        private LinearTickPosition[] logTickInfos;
        private TickFormattedText[] logTickLabels;
        private GridLinePosition[] gridLines;

        private void InitialzeTicks()
        {
            //logTicks = null;
            logTickInfos = null;
            logTickLabels = null;
            gridLines = null;

            tickLabelFormator = null;
            titleLabelFormator = null;

            UpdateAxisScale();
        }

        private void UpdateAxisScale()
        {
            if (AxisModel.Orientation == AxisOrientation.Horizontal)
            {
                //axis.StartPosistion = AxisModel.PlotLength * AxisModel.PlotStartPos;
                //axis.StopPosistion = AxisModel.PlotLength * AxisModel.PlotStopPos;
                axis.StartPosistion = AxisModel.PlotStartPos;
                axis.StopPosistion = AxisModel.PlotStopPos;
            }
            else
            {
                //axis.StartPosistion = AxisModel.PlotLength * AxisModel.PlotStopPos;
                //axis.StopPosistion = AxisModel.PlotLength * AxisModel.PlotStartPos;
                axis.StartPosistion = AxisModel.PlotStartPos;
                axis.StopPosistion = AxisModel.PlotStopPos;
            }
            if (LogarithmicAxixModel.Minimum.HasValue && LogarithmicAxixModel.Maximum.HasValue)
            {
                axis.Minimum = LogarithmicAxixModel.Minimum.Value;
                axis.Maximum = LogarithmicAxixModel.Maximum.Value;
            }
            else
            {

            }
            axis.Update();
        }

        private void PrepareTicks()
        {
            var logTicks = tickCreator.CreateTicks();
            CreateLogarithmicTickInfos(logTicks, out logTickInfos, out logTickLabels, out gridLines);
        }

        private void DrawTicksAtRight(DrawingContext dc)
        {
            PrepareTicks();

            double lineStartPos = axis.StopPosistion;
            double lineStopPos = axis.StartPosistion;

            double lineTotal = lineStopPos - lineStartPos;
            double longTickLength = LogarithmicAxixModel.MajorTickLength;
            double shortTickLength = LogarithmicAxixModel.MinorTickLength;
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

            if (logTickInfos != null && logTickInfos.Length > 0)
            {
                var tickOffset = AxisModel.TickBaseLinePen.Thickness / 2.0;
                foreach (var tick in logTickInfos)
                {
                    double mayorTop = tick.Position;
                    if (centerTicks)
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(baseLine - tick.Length / 2.0, mayorTop), new Point(baseLine + tick.Length / 2.0, mayorTop));
                    }
                    else
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(baseLine + tickOffset, mayorTop), new Point(baseLine + tickOffset + tick.Length, mayorTop));
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

            if (logTickLabels != null && logTickLabels.Length > 0)
            {
                double labelPos = leftPosition + AxisModel.LabelMargin;
                double labelTop = 0.0;
                foreach (var tick in logTickLabels)
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

            double lineStartPos = axis.StopPosistion;
            double lineStopPos = axis.StartPosistion;

            double lineTotal = lineStopPos - lineStartPos;
            double longTickLength = LogarithmicAxixModel.MajorTickLength;
            double shortTickLength = LogarithmicAxixModel.MinorTickLength;
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

            if (logTickLabels != null && logTickLabels.Length > 0)
            {
                double labelPos = leftPosition + AxisModel.LabelMargin;
                double labelTop = 0.0;
                foreach (var tick in logTickLabels)
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
                baseLine = leftPosition + longTickLength + AxisModel.TickBaseLinePen.Thickness / 2.0 ;
            }

            if (logTickInfos != null && logTickInfos.Length > 0)
            {
                var tickOffset = AxisModel.TickBaseLinePen.Thickness / 2.0;
                foreach (var tick in logTickInfos)
                {
                    double mayorTop = tick.Position;
                    if (centerTicks)
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(baseLine - tick.Length / 2.0, mayorTop), new Point(baseLine + tick.Length / 2.0, mayorTop));
                    }
                    else
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(baseLine - tick.Length - tickOffset, mayorTop), new Point(baseLine - tickOffset, mayorTop));
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
                leftPosition = baseLine + AxisModel.TickBaseLinePen.Thickness / 2.0;
            }

            renderLeft = 0.0;
            renderRight = leftPosition;
            renderTop = 0.0;
            renderBottom = AxisModel.PlotLength;
        }

        private void DrawTicksAtAbove(DrawingContext dc)
        {
            PrepareTicks();

            double lineStartPos = axis.StopPosistion;
            double lineStopPos = axis.StartPosistion;

            double lineTotal = lineStopPos - lineStartPos;
            double longTickLength = LogarithmicAxixModel.MajorTickLength;
            double shortTickLength = LogarithmicAxixModel.MinorTickLength;
            bool centerTicks = (AxisModel.ContentLayout & AxisContentLayout.TicksCentered) != 0;

            double topPosition = AxisModel.TitleMargin;

            double titleTop = 0;
            double titleCenter = (lineStartPos + lineStopPos) / 2.0;

            var titleText = GetFormatedTitle();
            if (AxisModel.ShowTitle && titleText != null)
            {
                var titlePos = titleCenter - titleText.Width / 2.0;
                dc.DrawText(titleText, new Point(titlePos, titleTop));
                topPosition = titleTop + titleText.Height;
            }

            if (logTickLabels != null && logTickLabels.Length > 0)
            {
                double labelTop = topPosition + AxisModel.LabelMargin;
                double labelPos = 0;
                foreach (var tick in logTickLabels)
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

            if (logTickInfos != null && logTickInfos.Length > 0)
            {
                var tickoffset = AxisModel.TickBaseLinePen.Thickness / 2.0;
                foreach (var tick in logTickInfos)
                {
                    double tickPos = tick.Position;
                    if (centerTicks)
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(tickPos, baseLine - tick.Length / 2), new Point(tickPos, baseLine + tick.Length / 2));
                    }
                    else
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(tickPos, baseLine - tickoffset), new Point(tickPos, baseLine - tick.Length - tickoffset));
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
                topPosition = baseLine + AxisModel.TickBaseLinePen.Thickness / 2.0;
            }

            topPosition = baseLine + AxisModel.TickBaseLinePen.Thickness;

            renderLeft = 0.0;
            renderRight = AxisModel.PlotLength;
            renderTop = 0.0;
            renderBottom = topPosition;
        }

        private void DrawTicksAtBelow(DrawingContext dc)
        {
            PrepareTicks();

            double lineStartPos = axis.StartPosistion;
            double lineStopPos = axis.StopPosistion;

            double lineTotal = lineStopPos - lineStartPos;

            double longTickLength = LogarithmicAxixModel.MajorTickLength;
            double shortTickLength = LogarithmicAxixModel.MinorTickLength;
            bool centerTicks = (AxisModel.ContentLayout & AxisContentLayout.TicksCentered) != 0;

            double topPosition = 0.0;

            double baseLine;
            if (centerTicks)
            {
                baseLine = topPosition + longTickLength / 2;
            }
            else
            {
                baseLine = topPosition + AxisModel.TickBaseLinePen.Thickness / 2;
            }
            if (logTickInfos != null && logTickInfos.Length > 0)
            {
                var tickOffset = AxisModel.TickBaseLinePen.Thickness / 2.0;
                foreach (var tick in logTickInfos)
                {
                    double tickPos = tick.Position;
                    if (centerTicks)
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(tickPos, baseLine - tick.Length / 2.0), new Point(tickPos, baseLine + tick.Length / 2.0));
                    }
                    else
                    {
                        dc.DrawLine(AxisModel.TickPen, new Point(tickPos, baseLine + tickOffset), new Point(tickPos, baseLine + tickOffset + tick.Length));
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

            if (logTickLabels != null && logTickLabels.Length > 0)
            {
                double labelTop = topPosition + AxisModel.LabelMargin;
                double labelPos = 0;
                foreach (var tick in logTickLabels)
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
                double titleCenter = (lineStartPos + lineStopPos) / 2.0;
                var titlePos = titleCenter - titleText.Width / 2.0;
                dc.DrawText(titleText, new Point(titlePos, topPosition));
                topPosition = topPosition + titleText.Height + AxisModel.TitleMargin;
            }

            renderLeft = 0.0;
            renderRight = AxisModel.PlotLength;
            renderTop = 0.0;
            renderBottom = topPosition;
        }


        private void CreateLogarithmicTickInfos(LogarithmicTick[] ticks, out LinearTickPosition[] tickInfos, out TickFormattedText[] tickLabels, out GridLinePosition[] lines)
        {
            var tickInfoList = new List<LinearTickPosition>();
            var tickLabelList = new List<TickFormattedText>();
            var gridLines = new List<GridLinePosition>();


            if (ticks != null && ticks.Length > 0)
            {
                foreach (var tick in ticks)
                {
                    if (tick.TickVisible)
                    {
                        var t = new LinearTickPosition()
                        {
                            Value = tick.Value.Value,
                            Position = axis.GetPositionByValue(tick.Value.Value),
                            Length = LogarithmicAxixModel.MajorTickLength,
                        };
                        tickInfoList.Add(t);
                        if (tick.LabelVisible)
                        {
                            var text = new TickFormattedText()
                            {
                                Position = t.Position,
                                Text = GetLogarithmicTickLabelFormator().GetFormattedText((AxisModel.DataConverter != null ? AxisModel.DataConverter.ToDataValue(tick.Value.Value) : tick.Value.Value)),
                            };
                            tickLabelList.Add(text);
                        }
                        gridLines.Add(new GridLinePosition(t.Position, true));
                    }
                }
            }
            tickInfos = tickInfoList.ToArray();
            tickLabels = tickLabelList.ToArray();
            lines = gridLines.ToArray();
        }

        private GridLinePosition[] CreateGridLines(LogarithmicTick[] ticks)
        {
            var ret = new List<GridLinePosition>();

            if (ticks != null && ticks.Length > 0)
            {
                foreach (var tick in ticks)
                {
                    if (tick.TickVisible)
                    {
                        ret.Add(new GridLinePosition(axis.GetPositionByValue(tick.Value.Value), true));
                    }
                }
            }
            return ret.ToArray();
        }

        #endregion

        private int GetMaxTickCount()
        {
            var actualExtent = Math.Abs(axis.StopPosistion - axis.StartPosistion);
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

        private double GetLabelCheckSize(FormattedText text)
        {
            return AxisModel.Orientation == AxisOrientation.Horizontal ? text.Width : text.Height;
        }

        private double GetIncreaseRatio()
        {
            return AxisModel.Orientation == AxisOrientation.Horizontal ? 1.5 : 1.05;
        }

        private double GetDecreaseRatio()
        {
            return AxisModel.Orientation == AxisOrientation.Horizontal ? 1.2 : 1.01;
        }

        private TickCountChange CheckLabelsArrangement(TickFormattedText[] tickTexts)
        {
            var ret = tickTexts.Length != 0 ? TickCountChange.OK : TickCountChange.Increase;

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
