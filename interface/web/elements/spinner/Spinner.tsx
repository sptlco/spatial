// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new spinner element.
 * @param props Configurable options for the element.
 * @returns A spinner element.
 */
export const Spinner: Element = (props: ElementProps): Node => {
  return (
    <svg
      ref={props.ref}
      style={props.style}
      viewBox="0 0 32 33"
      xmlns="http://www.w3.org/2000/svg"
      className={clsx(
        "transition-transform",
        "animate-spin ease-in-out",
        props.className,
      )}
    >
      <path
        fillRule="evenodd"
        clipRule="evenodd"
        className="fill-[currentColor] opacity-10"
        d="M16 3.41772C12.5522 3.41772 9.24558 4.78736 6.80761 7.22534C4.36964 9.66331 3 12.9699 3 16.4177C3 19.8655 4.36964 23.1721 6.80761 25.6101C9.24558 28.0481 12.5522 29.4177 16 29.4177C19.4478 29.4177 22.7544 28.0481 25.1924 25.6101C27.6304 23.1721 29 19.8655 29 16.4177C29 12.9699 27.6304 9.66331 25.1924 7.22534C22.7544 4.78736 19.4478 3.41772 16 3.41772ZM0 16.4177C0 12.1743 1.68571 8.1046 4.68629 5.10402C7.68687 2.10343 11.7565 0.417725 16 0.417725C20.2435 0.417725 24.3131 2.10343 27.3137 5.10402C30.3143 8.1046 32 12.1743 32 16.4177C32 20.6612 30.3143 24.7309 27.3137 27.7314C24.3131 30.732 20.2435 32.4177 16 32.4177C11.7565 32.4177 7.68687 30.732 4.68629 27.7314C1.68571 24.7309 0 20.6612 0 16.4177Z"
      />
      <path
        fillRule="evenodd"
        clipRule="evenodd"
        className="fill-[currentColor]"
        d="M14.5 1.91772C14.5 1.5199 14.658 1.13837 14.9393 0.857064C15.2206 0.57576 15.6022 0.417725 16 0.417725C20.2435 0.417725 24.3131 2.10343 27.3137 5.10402C30.3143 8.1046 32 12.1743 32 16.4177C32 16.8155 31.842 17.1971 31.5607 17.4784C31.2794 17.7597 30.8978 17.9177 30.5 17.9177C30.1022 17.9177 29.7206 17.7597 29.4393 17.4784C29.158 17.1971 29 16.8155 29 16.4177C29 12.9699 27.6304 9.66331 25.1924 7.22534C22.7544 4.78736 19.4478 3.41772 16 3.41772C15.6022 3.41772 15.2206 3.25969 14.9393 2.97838C14.658 2.69708 14.5 2.31555 14.5 1.91772Z"
      />
    </svg>
  );
};
