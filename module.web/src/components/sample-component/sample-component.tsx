import { Component, Prop, h } from '@stencil/core';
@Component({
  tag: 'sample-component',
  styleUrl: 'sample-component.scss',
  shadow: true,
})
export class SampleComponent {
  /**
   * The first name
   */
  @Prop() first: string;

  /**
   * The middle name
   */
  @Prop() middle: string;

  /**
   * The last name
   */
  @Prop() last: string;

  render() {
    return <div>Hello, World! I'm {this.first} {this.middle} {this.last}</div>;
  }
}
