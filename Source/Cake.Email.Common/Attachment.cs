using HeyRed.Mime;
using System;
using System.IO;

namespace Cake.Email.Common
{
	/// <summary>
	/// Represents an attachment to an e-mail.
	/// </summary>
	public class Attachment : IDisposable
	{
		private bool _isDisposed = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="Attachment"/> class
		/// with the specified Stream and mime type.
		/// </summary>
		/// <param name="contentStream">A stream containing the content for this attachment.</param>
		/// <param name="name">The name of this attachment.</param>
		/// <param name="mimeType">The MIME media type of the content.</param>
		/// <param name="contentId">The ID of the content. Only necessary if the attachment is intended to be inline.</param>
		/// <exception cref="ArgumentNullException">contentStream is null.</exception>
		public Attachment(Stream contentStream, string name, string mimeType, string contentId = null)
		{
			this.ContentStream = contentStream ?? throw new ArgumentNullException(nameof(contentStream));
			this.Name = name;
			this.MimeType = !string.IsNullOrEmpty(mimeType) ? mimeType : throw new ArgumentException("mimeType is empty", nameof(mimeType));
			this.ContentId = contentId;
		}

		/// <summary>
		/// Gets or sets the content stream of this attachment.
		/// </summary>
		public Stream ContentStream { get; set; }

		/// <summary>
		/// Gets or sets the name of this attachment.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the MIME type of this attachment.
		/// </summary>
		public string MimeType { get; set; }

		/// <summary>
		/// Gets or sets the MIME content ID for this attachment.
		/// </summary>
		public string ContentId { get; set; }

		/// <summary>
		/// Convenience method that creates an attachment from a local file.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <param name="mimeType">Optional: MIME type of the attachment. If this parameter is null, the MIME type will be derived from the file extension.</param>
		/// <param name="contentId">Optional: the unique identifier for this attachment IF AND ONLY IF the attachment should be embedded in the body of the email. This is useful, for example, if you want to embbed an image to be displayed in the HTML content. For standard attachment, this value should be null.</param>
		/// <returns>The attachment.</returns>
		/// <exception cref="System.IO.FileNotFoundException">Unable to find the local file.</exception>
		public static Attachment FromLocalFile(string filePath, string mimeType = null, string contentId = null)
		{
			var fileInfo = new FileInfo(filePath);
			if (!fileInfo.Exists)
			{
				throw new FileNotFoundException("Unable to find the local file", filePath);
			}

			if (string.IsNullOrEmpty(mimeType))
			{
				mimeType = MimeTypesMap.GetMimeType(filePath);
			}

			var content = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

			return new Attachment(content, fileInfo.Name, mimeType, contentId);
		}

		/// <summary>
		/// Convenience method that creates an attachment from a stream.
		/// </summary>
		/// <param name="contentStream">The stream.</param>
		/// <param name="name">The name of the attachment.</param>
		/// <param name="mimeType">Optional: MIME type of the attachment. If this parameter is null, the MIME type will be derived from the fileName extension.</param>
		/// <param name="contentId">Optional: the unique identifier for this attachment IF AND ONLY IF the attachment should be embedded in the body of the email. This is useful, for example, if you want to embbed an image to be displayed in the HTML content. For standard attachment, this value should be null.</param>
		/// <returns>The attachment.</returns>
		public static Attachment FromStream(Stream contentStream, string name, string mimeType = null, string contentId = null)
		{
			if (string.IsNullOrEmpty(mimeType))
			{
				mimeType = MimeTypesMap.GetMimeType(name);
			}

			return new Attachment(contentStream, name, mimeType, contentId);
		}

		/// <summary>
		/// Releases the resources Attachment.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}

		/// <summary>
		/// Releases the unmanaged resources used by the Attachment and
		/// optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !_isDisposed)
			{
				_isDisposed = true;
				ContentStream.Dispose();
			}
		}
	}
}
