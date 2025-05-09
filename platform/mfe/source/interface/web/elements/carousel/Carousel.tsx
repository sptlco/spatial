// Copyright © Spatial. All rights reserved.

"use client";

import { clsx } from "clsx";
import {
  Button,
  ButtonLabel,
  CarouselProps,
  Div,
  Element,
  Icon,
  Node,
} from "..";
import useEmblaCarousel from "embla-carousel-react";
import Autoplay from "embla-carousel-autoplay";
import Autoscroll from "embla-carousel-auto-scroll";
import Autoheight from "embla-carousel-auto-height";
import { WheelGesturesPlugin as WheelGestures } from "embla-carousel-wheel-gestures";
import { cva } from "class-variance-authority";
import { EmblaCarouselType, EmblaPluginType } from "embla-carousel";
import { useCallback, useEffect, useState } from "react";

const classes = cva(["flex items-start"], {
  variants: {
    orientation: {
      horizontal: [],
      vertical: ["flex-col"],
    },
  },
});

/**
 * Create a new carousel element.
 * @param props Configurable options for the element.
 * @returns A carousel element.
 */
export const Carousel: Element<CarouselProps> = ({
  loop = true,
  autoPlay = false,
  autoScroll = false,
  orientation = "horizontal",
  direction = "forward",
  ...props
}: CarouselProps): Node => {
  const plugins: EmblaPluginType[] = [Autoheight(), WheelGestures()];

  if (autoPlay) {
    plugins.push(Autoplay());
  }

  if (autoScroll) {
    plugins.push(
      Autoscroll({
        direction: direction,
        stopOnInteraction: props.stopOnInteraction,
        stopOnMouseEnter: props.stopOnMouseEnter,
        stopOnFocusIn: props.stopOnFocusIn,
      }),
    );
  }

  const [emblaRef, emblaApi] = useEmblaCarousel(
    {
      loop,
      axis: orientation == "horizontal" ? "x" : "y",
    },
    plugins,
  );

  const [prevButtonDisabled, setPrevButtonDisabled] = useState(true);
  const [nextButtonDisabled, setNextButtonDisabled] = useState(true);

  const select = useCallback((emblaApi: EmblaCarouselType) => {
    setPrevButtonDisabled(!emblaApi.canScrollPrev);
    setNextButtonDisabled(!emblaApi.canScrollNext);
  }, []);

  const previous = useCallback(() => {
    if (emblaApi) {
      emblaApi.scrollPrev();
    }
  }, [emblaApi]);

  const next = useCallback(() => {
    if (emblaApi) {
      emblaApi.scrollNext();
    }
  }, [emblaApi]);

  useEffect(() => {
    if (!emblaApi) {
      return;
    }

    select(emblaApi);
    emblaApi.on("reInit", select).on("select", select);
  }, [emblaApi, select]);

  return (
    <Div ref={props.ref} style={props.style} className="w-full">
      <Div
        className={clsx("embla", "gap-1u flex size-full items-center", {
          "flex-col": orientation == "vertical",
        })}
      >
        {props.showButtons && (
          <Button
            intent="outline"
            onClick={previous}
            disabled={prevButtonDisabled}
            className={clsx(
              "embla__prev !p-1u !h-fit !w-fit !min-w-0 !rounded-full",
              {
                "rotate-90": orientation == "vertical",
              },
            )}
          >
            <Icon icon="arrow_left_alt" />
          </Button>
        )}
        <Div
          ref={emblaRef}
          className={clsx("embla__viewport", "size-full overflow-hidden")}
        >
          <Div
            children={props.children}
            className={clsx(
              "embla__container",
              classes({ orientation }),
              props.className,
            )}
          />
        </Div>
        {props.showButtons && (
          <Button
            intent="outline"
            onClick={next}
            disabled={nextButtonDisabled}
            className={clsx(
              "embla__next !p-1u !h-fit !w-fit !min-w-0 !rounded-full",
              {
                "rotate-90": orientation == "vertical",
              },
            )}
          >
            <Icon icon="arrow_right_alt" />
          </Button>
        )}
      </Div>
    </Div>
  );
};
