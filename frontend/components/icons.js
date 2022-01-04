export const LoadingIcon = ({ className }) => {
  return (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      className={className}
      viewBox="0 0 24 24"
      fill="none"
    >
      <circle
        className="opacity-25"
        cx="12"
        cy="12"
        r="10"
        stroke="currentColor"
        strokeWidth="4"
      ></circle>
      <path
        className="opacity-75"
        fill="currentColor"
        d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
      ></path>
    </svg>
  );
};

export const DropboxIcon = ({ className }) => {
  return (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      className={className}
      viewBox="0 0 24 24"
      fill="currentColor"
    >
      <path d="M6 1.807L0 5.629l6 3.822 6.001-3.822L6 1.807zM18 1.807l-6 3.822 6 3.822 6-3.822-6-3.822zM0 13.274l6 3.822 6.001-3.822L6 9.452l-6 3.822zM18 9.452l-6 3.822 6 3.822 6-3.822-6-3.822zM6 18.371l6.001 3.822 6-3.822-6-3.822L6 18.371z" />
    </svg>
  );
};

export const YoutubeIcon = ({ className }) => {
  return (
    <svg
      className={className}
      viewBox="0 0 32 32"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
      
    >
      <g clipPath="url(#clip0_4_206)">
        <path
          d="M31.3627 8.24728C31.1817 7.5665 30.8252 6.94517 30.3287 6.44549C29.8322 5.9458 29.2132 5.58528 28.5336 5.40001C26.0317 4.72728 16.0317 4.72728 16.0317 4.72728C16.0317 4.72728 6.03174 4.72728 3.52992 5.40001C2.85031 5.58528 2.23128 5.9458 1.73479 6.44549C1.23829 6.94517 0.881746 7.5665 0.700829 8.24728C0.0317384 10.76 0.0317383 16 0.0317383 16C0.0317383 16 0.0317384 21.24 0.700829 23.7527C0.881746 24.4335 1.23829 25.0548 1.73479 25.5545C2.23128 26.0542 2.85031 26.4147 3.52992 26.6C6.03174 27.2727 16.0317 27.2727 16.0317 27.2727C16.0317 27.2727 26.0317 27.2727 28.5336 26.6C29.2132 26.4147 29.8322 26.0542 30.3287 25.5545C30.8252 25.0548 31.1817 24.4335 31.3627 23.7527C32.0317 21.24 32.0317 16 32.0317 16C32.0317 16 32.0317 10.76 31.3627 8.24728Z"
          fill="currentColor"
        />
        <path
          d="M12.759 20.7582V11.2419L21.1227 16L12.759 20.7582Z"
          fill="#FEFEFE"
        />
      </g>
      <defs>
        <clipPath id="clip0_4_206">
          <rect width="32" height="32" fill="white" />
        </clipPath>
      </defs>
    </svg>
  );
};
